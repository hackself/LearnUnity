using System.Collections;
using UnityEngine;
using System.Diagnostics;

namespace ObjectTest
{
    public class Program : MonoBehaviour
    {

#if UNITY_EDITOR
        private void Start()
        {
            this.StartCoroutine(Test(this.gameObject));
        }
#else
        static void Main(string[] args)
        {
            
            Application.Instance.Run(false,() =>
            {
                var obj = new GameObject("test");
                var test = obj.AddComponent<Test>();
                //测试协程
                test.StartCoroutine(Test(obj));
            });
        }
#endif

        static WaitForSeconds PrintWait(float second)
        {
#if !UNITY_EDITOR
            SceneManager.Instance.currentScene.PrintHierarchy();
#endif
            return new WaitForSeconds(second);
        }

        static IEnumerator Test(GameObject obj)
        {
            yield return new WaitForSeconds(1);

            //测试创建GameObject和设置父子关系接口
            for (int i = 0; i < 2; ++i)
            {
                var child_1 = new GameObject("Obj" + i,typeof(Test_1));
                child_1.transform.parent = obj.transform;
                for (int j = 0; j < 2; ++j)
                {
                    var child_2 = new GameObject("Obj" + j, typeof(Test_2));
                    child_2.transform.parent = child_1.transform;
                }
                yield return PrintWait(1);
            }

            //测试Find接口
            var t = obj.transform.Find("Obj0");

            //测试激活接口
            t.gameObject.SetActive(false);
            yield return PrintWait(1);
            t.gameObject.SetActive(true);
            yield return PrintWait(1);

            //测试删除接口
            Object.DestroyImmediate(t.gameObject);
            yield return PrintWait(1);

            t = obj.transform.Find("Obj1/Obj1");
            Object.DestroyImmediate(t.gameObject.GetComponent<Test_2>());
            yield return PrintWait(1);
        }
    }
}
