// -----------------------------------------------------------------
// File:    Time
// Author:  ruanban
// Date:    1/4/2018 5:41:08 PM
// Description:
//      
// -----------------------------------------------------------------

using System;

namespace UnityEngine
{
    public sealed class Time
    {
        //游戏启动时的系统时间
        private static DateTime _appStartTime;
        //上一帧结束系统时间
        private static DateTime _frameStartTime;
        //每fixedDeltaTime，会产生一个fixedUpdate,多余的时间用这个变量存着
        private static float _fixedDeltaTime_;
        //上一个选择帧过后，产生的固定帧数量
        private static int _fixedDeltaFrameCount_;

        public static float time { get; private set; }
        public static float deltaTime { get; private set; }
        public static float fixedTime { get; private set; }

        //固定帧，间隔时间。Unity中可以在编辑器中设置
        public static float fixedDeltaTime { get; set; }

        //渲染帧数
        public static int renderedFrameCount { get; private set; }

        //固定帧数
        public static int frameCount { get; private set; }

        public static bool inFixedTimeStep { get; private set; }

        public Time()
        {
            _frameStartTime = DateTime.Now;
            _appStartTime = _frameStartTime;

            //每秒25帧
            fixedDeltaTime = 0.04f; 
            _fixedDeltaFrameCount_ = 0;
            inFixedTimeStep = false;
        }

        static private float _deltaSeconds(DateTime from,DateTime to)
        {
            return (float)((to - from).TotalSeconds);
        }

        static public void BeginFrame()
        {
            time = _deltaSeconds(_appStartTime, DateTime.Now);
        }

        static public void EndFrame()
        {
            if (inFixedTimeStep)
                --_fixedDeltaFrameCount_;

            deltaTime = _deltaSeconds(_frameStartTime,DateTime.Now);
            _fixedDeltaTime_ += deltaTime;
            _fixedDeltaFrameCount_ += (int)(_fixedDeltaTime_ / fixedDeltaTime);
            _fixedDeltaTime_ = _fixedDeltaTime_ - (_fixedDeltaFrameCount_ * fixedDeltaTime);
            ++renderedFrameCount;

            inFixedTimeStep = _fixedDeltaFrameCount_ > 0;
            if (inFixedTimeStep)
                ++frameCount;
        }
    }
}
