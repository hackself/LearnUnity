using System;
using UnityEngine;
using UnityEngine.UI;
using NativeDllTest;
using System.Reflection;

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

        Op_begin,
        Add,
        Sub,
        Mul,
        Div,
        Op_End,
        Op_Null = Op_begin,

        Equal
    }

    //当前的输入状态
    private enum State
    {
        input_num_1,
        input_num_2,
        DisplayResult,
        Error
    }

    [SerializeField]
    private Transform keyboard = null;
    [SerializeField]
    private Text displayText = null;

    private string display
    {
        set { this.displayText.text = value; }
        get { return this.displayText.text; }
    }

    private State state = State.input_num_1;

    private Keyboard op;
    private double num_1;
    private double num_2;

    private object[] op_param = new object[2];

    private bool IsError()
    {
        return this.state == State.Error;
    }

    private void Error(string str)
    {
        this.display = str;
        this.state = State.Error;
    }

    private void DoCalc()
    {
        if (this.op > Keyboard.Op_begin && this.op < Keyboard.Op_End)
        {
            if (this.op == Keyboard.Div && num_2 == 0)
            {
                this.Error("除数不能为0");
                return;
            }
            var opMethod = typeof(NativeMath).GetMethod(this.op.ToString(),
                BindingFlags.Static |
                BindingFlags.InvokeMethod |
                BindingFlags.NonPublic |
                BindingFlags.Public);
            if (opMethod != null)
            {
                op_param[0] = num_1;
                op_param[1] = num_2;
                num_1 = (double)opMethod.Invoke(null, op_param);
                this.display = num_1.ToString();
                this.state = State.DisplayResult;
            }
            this.Error("无效的操作符");
        }
    }

    private void Awake()
    {
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

    private void OnClickKeyboard(Keyboard key)
    {
        if (this.IsError() && (key != Keyboard.C || key != Keyboard.CE))
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
        else if (key > Keyboard.Op_begin && key < Keyboard.Op_End)
            this.OnClickOperation(key);
    }

    private void ClearDisplay()
    {
        this.display = "0";
    }

    private bool IsDisplayClear()
    {
        return this.display == "0";
    }

    private void SaveInput()
    {
        if (this.state == State.input_num_1)
        {
            this.num_1 = Convert.ToDouble(this.display);
        }
        else if (this.state == State.input_num_2)
        {
            this.num_2 = Convert.ToDouble(this.display);
        }
    }

    //删除
    private void OnClickDel()
    {
        if (this.state == State.DisplayResult) return;
        if (display.Length > 1)
            this.display = this.display.Substring(0, this.display.Length - 1);
        else
            this.ClearDisplay();
    }

    //清除
    private void Clear()
    {
        this.op = Keyboard.Op_Null;
        this.state = State.input_num_1;
        this.num_1 = 0;
        this.num_2 = 0;
        this.ClearDisplay();
    }

    //清除错误
    private void OnClickClearError()
    {
        if (this.IsError())
            this.Clear();
        else
            this.ClearDisplay();
    }

    //输入数字
    private void OnClickNum(Keyboard num)
    {
        if (this.IsDisplayClear())
        {
            if (num == Keyboard.Num_0)
                return;
            this.display = ((int)num).ToString();
        }
        else
        {
            this.display = this.display + (int)num;
        }
    }

    //输入小数点
    private void OnClickPoint()
    {
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

    //点击运算操作符
    private void OnClickOperation(Keyboard op)
    {
        this.op = op;
        this.SaveInput();
    }

    //计算
    private void OnClickEqual()
    {
        if (this.op == Keyboard.Op_Null) return;
        this.DoCalc();
    }
}
