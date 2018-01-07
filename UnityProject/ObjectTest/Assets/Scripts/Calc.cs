using System;
using UnityEngine;
using UnityEngine.UI;
using NativeDllTest;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

public class Calc : MonoBehaviour
{
    private enum Keyboard
    {
        Num_0 = 0,
        Num_1,
        Num_2,
        Num_3,
        Num_4,
        Num_5,
        Num_6,
        Num_7,
        Num_8,
        Num_9,

        Del,
        CE,
        C,

        Point,
        Sign,

        Op_Null, //无效操作符
        Op_Begin,/*===操作符开始===*/

        Op_Unary_Begin,         //一元操作符开始
        Power_2,   //求平方
        Sqrt_2,    //开平方根
        Op_Unary_End,           //一元操作符结束

        Op_Dyadic_Begin,    //二元操作符开始
        Add,
        Sub,
        Mul,
        Div,
        Op_Dyadic_End,      //二元操作符结束

        Op_End, /*===操作符结束===*/
        Equal,


    }

    private enum DisplayState
    {
        Input,
        SaveInput,
        Error,
    }

    private enum OpType
    {
        Op,
        Num
    }

    private class OpItem
    {
        public OpType type;
        public object obj;
        public OpItem(OpType type,object obj)
        {
            this.type = type;
            this.obj = obj;
        }
    }

    private class OpStack : List<OpItem>
    {
        public void PushNum(double num)
        {
            this.Add(new OpItem(OpType.Num, num));
        }

        public void PushOp(Keyboard op)
        {
            this.Add(new OpItem(OpType.Op, op));
        }
        private void Modify(int index,OpType type,object obj)
        {
            var item = this[index];
            item.type = type;
            item.obj = obj;
        }
        
        public void ModifyNum(int index,double num)
        {
            this.Modify(index, OpType.Num, num);
        }

        public void Pop()
        {
            this.RemoveAt(this.Count - 1);
        }

        public OpItem Peek()
        {
            return this[this.Count - 1];
        }

        public bool Peek(Keyboard op)
        {
            if (this.Count > 0 && this.Peek().obj.Equals(op))
                return true;
            return false;
        }
    }

    private Dictionary<Keyboard, string> opText = new Dictionary<Keyboard, string>();

    [SerializeField]
    private Transform keyboard = null;
    [SerializeField]
    private Text displayText = null;
    [SerializeField]
    private Text stackText = null;

    private DisplayState displayState;

    private bool isError
    {
        get { return this.displayState == DisplayState.Error; }
    }

    private string display
    {
        set { this.displayText.text = value; }
        get { return this.displayText.text; }
    }

    private double displayNum
    {
        get { return Convert.ToDouble(this.display); }
    }

    private OpStack opStack = new OpStack();

    private void Error(string str)
    {
        this.display = str;
        this.displayState = DisplayState.Error;
    }

    private bool IsUnaryOp(Keyboard op)
    {
        return op > Keyboard.Op_Unary_Begin && op < Keyboard.Op_Unary_End;
    }

    private bool IsDyadic(Keyboard op)
    {
        return op > Keyboard.Op_Dyadic_Begin && op < Keyboard.Op_Dyadic_End;
    }

    private void SaveInput()
    {
        if (this.displayState == DisplayState.SaveInput) return;
        this.displayState = DisplayState.SaveInput;
        if (this.opStack.Count > 0 && this.opStack.Peek().type == OpType.Num)
            this.opStack.ModifyNum(this.opStack.Count - 1, this.displayNum);
        else
            this.opStack.PushNum(this.displayNum);
        this.UpdateStack();
    }

    private void UpdateStack()
    {
        var str = new StringBuilder();
        foreach (var item in this.opStack)
        {
            if (item.type == OpType.Op)
            {
                string op;
                if (!this.opText.TryGetValue((Keyboard)item.obj, out op))
                {
                    op = item.obj.ToString();
                }
                str.Append(op);
            }
            else
            {
                str.Append(item.obj);
            }
            str.Append(' ');
        }
        this.stackText.text = str.ToString();
    }

    private bool DoCalc()
    {
        if (this.opStack.Count < 2 || this.opStack[1].type != OpType.Op) return false;
        var op = (Keyboard)this.opStack[1].obj;
        if (op > Keyboard.Op_Begin && op < Keyboard.Op_End)
        {
            if (op == Keyboard.Div && this.opStack.Count >=3 && 
                this.opStack[2].type == OpType.Num && (double)this.opStack[2].obj == 0)
            {
                this.Error("除数不能为0");
                return false;
            }

            //操作数不够
            if (this.IsUnaryOp(op) && this.opStack.Count < 2) return false;
            if (this.IsDyadic(op) && this.opStack.Count < 3) return false;


            var opMethod = typeof(NativeMath).GetMethod(op.ToString(),
                BindingFlags.Static |
                BindingFlags.NonPublic |
                BindingFlags.Public);
            if (opMethod != null)
            {
                double result = 0;
                object[] param = null;
                if (this.IsUnaryOp(op))
                    param = new object[1] { this.opStack[0].obj };
                else if (this.IsDyadic(op))
                    param = new object[2] { this.opStack[0].obj,this.opStack[2].obj};
                result = (double)opMethod.Invoke(null, param);
                this.display = result.ToString();
                this.displayState = DisplayState.Input;
                return true;
            }
        }
        this.Error("无效的操作符");
        return false;
    }

    private void InitOpText()
    {
        this.opText.Add(Keyboard.Add, "+");
        this.opText.Add(Keyboard.Sub, "-");
        this.opText.Add(Keyboard.Mul, "*");
        this.opText.Add(Keyboard.Div, "/");
        this.opText.Add(Keyboard.Equal, "=");
    }

    private void Awake()
    {
        this.InitOpText();

        this.Clear();

        //为所有按键添加事件
        var count = this.keyboard.childCount;
        for (int i = 0; i < count; ++i)
        {
            var child = this.keyboard.GetChild(i);
            var btn = child.GetComponent<Button>();
            btn.onClick.AddListener(()=> 
            {
                //通过名字获取枚举值
                var key = (Keyboard)Enum.Parse(typeof(Keyboard), btn.name);
                this.OnClickKeyboard(key);
            });
        }
    }

    private void SaveDisplay()
    {
        if (this.displayState == DisplayState.SaveInput)
        {
            this.ClearDisplay();
            return;
        }
        if (this.opStack.Count > 0 && this.opStack.Peek().type == OpType.Op)
        {
            if (this.opStack.Peek().obj.Equals(Keyboard.Equal))
            {
                this.Clear();
            }
            else
            {
                this.SaveInput();
                this.ClearDisplay();
            }
        }
    }

    private void OnClickKeyboard(Keyboard key)
    {
        if (this.isError && (key != Keyboard.C && key != Keyboard.CE))
            return;
        //分类处理
        if (key == Keyboard.Del)
            this.OnClickDel();
        else if (key == Keyboard.C)
            this.Clear();
        else if (key == Keyboard.CE)
            this.OnClickClearError();
        else if (key >= Keyboard.Num_0 && key <= Keyboard.Num_9)
            this.OnClickNum(key);
        else if (key == Keyboard.Point)
            this.OnClickPoint();
        else if (key == Keyboard.Sign)
            this.OnClickSign();
        else if (key == Keyboard.Equal)
            this.OnClickEqual();
        else if (key > Keyboard.Op_Begin && key < Keyboard.Op_End)
            this.OnClickOperation(key);
    }

    private void ClearDisplay()
    {
        this.display = "0";
        this.displayState = DisplayState.Input;
    }

    private bool IsDisplayClear()
    {
        return this.display == "0";
    }

    //删除
    private void OnClickDel()
    {
        if (this.opStack.Peek(Keyboard.Equal)) return;
            if (display.Length > 1)
            this.display = this.display.Substring(0, this.display.Length - 1);
        else
            this.ClearDisplay();
    }

    //清除
    private void Clear()
    {
        this.opStack.Clear();
        this.ClearDisplay();
        this.UpdateStack();
    }

    //清除错误
    private void OnClickClearError()
    {
        if (this.isError)
            this.Clear();
        else
            this.ClearDisplay();
        this.UpdateStack();
    }

    //输入数字
    private void OnClickNum(Keyboard num)
    {
        if (this.displayState == DisplayState.SaveInput)
            this.ClearDisplay();
        if (this.IsDisplayClear())
            this.display = ((int)num).ToString();
        else
            this.display = this.display + (int)num;
    }

    //输入小数点
    private void OnClickPoint()
    {
        this.SaveDisplay();
        if (this.display.IndexOf('.') != -1) return;
        this.display = this.display + ".";
    }

    //变换正负符号
    private void OnClickSign()
    {
        if (this.IsDisplayClear()) return;
        if (this.display.StartsWith("-"))
            this.display = this.display.Substring(1);
        else
            this.display = "-" + this.display;
    }

    //点击运算操作符按钮
    private void OnClickOperation(Keyboard op)
    {
        //修改操作符
        if (this.opStack.Peek(Keyboard.Equal))
        {
            this.opStack.Clear();
            this.SaveInput();
        }
        else
        {
            this.SaveInput();
            if (this.opStack.Count > 0 && this.opStack.Peek().type == OpType.Op)
                this.opStack.Pop();
        }
        if (this.DoCalc())
        {
            this.opStack.Clear();
            this.SaveInput();
        }
        this.opStack.PushOp(op);
        this.UpdateStack();
    }

    //计算
    private void OnClickEqual()
    {
        if (this.opStack.Peek(Keyboard.Equal))
        {
            this.opStack.ModifyNum(0, this.displayNum);
            this.DoCalc();
        }
        else
        {
            this.SaveInput();
            if (this.DoCalc())
            {
                this.opStack.PushOp(Keyboard.Equal);
            }
        }
        this.UpdateStack();
    }
}
