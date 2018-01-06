LOCAL_PATH := $(call my-dir)
include $(CLEAR_VARS)
LOCAL_MODULE := NativeDllTest
LOCAL_SRC_FILES := ../NativeDllTest.cpp
include $(BUILD_SHARED_LIBRARY)
APP_ABI := armeabi-v7a x86
