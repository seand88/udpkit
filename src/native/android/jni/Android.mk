LOCAL_PATH := $(call my-dir)  
include $(CLEAR_VARS)  
LOCAL_LDLIBS 	:= -llog  
LOCAL_MODULE    := udpkit_android
LOCAL_SRC_FILES := ../../shared/common.cpp ../../shared/socket.cpp ../../shared/timers.cpp
include $(BUILD_SHARED_LIBRARY) 