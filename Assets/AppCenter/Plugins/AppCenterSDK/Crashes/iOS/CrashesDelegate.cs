﻿// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using Microsoft.AppCenter.Unity.Crashes;

namespace Microsoft.AppCenter.Unity.Crashes.Internal
{
    public class CrashesDelegate
    {
        static CrashesDelegate()
        {
            delegateShouldProcesReport = ShouldProcessErrorReportNativeFunc;
            app_center_unity_crashes_delegate_set_should_process_error_report_delegate(delegateShouldProcesReport);
            delegateGetAttachments = GetErrorAttachmentsNativeFunc;
            app_center_unity_crashes_delegate_set_get_error_attachments_delegate(delegateGetAttachments);
            delegateSendingErrorReport = SendingErrorReportNativeFunc;
            app_center_unity_crashes_delegate_set_sending_error_report_delegate(delegateSendingErrorReport);
            delegateSentErrorReport = SentErrorReportNativeFunc;
            app_center_unity_crashes_delegate_set_sent_error_report_delegate(delegateSentErrorReport);
            delegateFailedToSendErrorReport = FailedToSendErrorReportNativeFunc;
            app_center_unity_crashes_delegate_set_failed_to_send_error_report_delegate(delegateFailedToSendErrorReport);
        }

        public static void SetDelegate()
        {
            app_center_unity_crashes_set_delegate();
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate bool NativeShouldProcessErrorReportDelegate(IntPtr report);
        public static NativeShouldProcessErrorReportDelegate delegateShouldProcesReport;
        private static Crashes.ShouldProcessErrorReportHandler shouldProcessReportHandler = null;

        [MonoPInvokeCallback(typeof(NativeShouldProcessErrorReportDelegate))]
        public static bool ShouldProcessErrorReportNativeFunc(IntPtr report)
        {
            if (shouldProcessReportHandler != null)
            {
                ErrorReport errorReport = CrashesInternal.GetErrorReportFromIntPtr(report); 
                return shouldProcessReportHandler(errorReport);   
            }
            else
            {
                return true;
            }
        }

        public static void SetShouldProcessErrorReportHandler(Crashes.ShouldProcessErrorReportHandler handler)
        {
            shouldProcessReportHandler = handler;
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate IntPtr NativeGetErrorAttachmentsDelegate(IntPtr report);
        public static NativeGetErrorAttachmentsDelegate delegateGetAttachments;
        private static Crashes.GetErrorAttachmentsHandler getErrorAttachmentsHandler = null;

        [MonoPInvokeCallback(typeof(NativeGetErrorAttachmentsDelegate))]
        public static IntPtr GetErrorAttachmentsNativeFunc(IntPtr report)
        {
            if (getErrorAttachmentsHandler != null)
            {
                var errorReport = CrashesInternal.GetErrorReportFromIntPtr(report); 
                var logs = getErrorAttachmentsHandler(errorReport);
                var nativeLogs = new List<IntPtr>();
                foreach (var errorAttachmetLog in logs)
                {
                    IntPtr nativeLog = IntPtr.Zero;
                    if (errorAttachmetLog.Type == ErrorAttachmentLog.AttachmentType.Text)
                    {
                        nativeLog = app_center_unity_crashes_get_error_attachment_log_text(errorAttachmetLog.Text, errorAttachmetLog.FileName);
                    }
                    else
                    {
                        nativeLog = app_center_unity_crashes_get_error_attachment_log_binary(errorAttachmetLog.Data, errorAttachmetLog.Data.Length, errorAttachmetLog.FileName, errorAttachmetLog.ContentType);
                    }
                    nativeLogs.Add(nativeLog);
                }

                IntPtr log0 = IntPtr.Zero;
                if (nativeLogs.Count > 0)
                {   
                    log0 = nativeLogs[0];
                }
                IntPtr log1 = IntPtr.Zero;
                if (nativeLogs.Count > 1) 
                {
                    log1 = nativeLogs[1];
                }
                return app_center_unity_create_error_attachments_array(log0, log1);
            }
            else
            {
                return IntPtr.Zero;
            }   
        }

        public static void SetGetErrorAttachmentsHandler(Crashes.GetErrorAttachmentsHandler handler)
        {
            getErrorAttachmentsHandler = handler;
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate void NativeSendingErrorReportDelegate(IntPtr report);
        public static NativeSendingErrorReportDelegate delegateSendingErrorReport;
        private static Crashes.SendingErrorReportHandler sendingErrorReportHandler = null;

        [MonoPInvokeCallback(typeof(NativeSendingErrorReportDelegate))]
        public static void SendingErrorReportNativeFunc(IntPtr report)
        {
            if (sendingErrorReportHandler != null)
            {
                ErrorReport errorReport = CrashesInternal.GetErrorReportFromIntPtr(report);
                sendingErrorReportHandler(errorReport);   
            }
        }

        public static void SetSendingErrorReportHandler(Crashes.SendingErrorReportHandler handler)
        {
            sendingErrorReportHandler = handler;
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate void NativeSentErrorReportDelegate(IntPtr report);
        public static NativeSentErrorReportDelegate delegateSentErrorReport;
        private static Crashes.SentErrorReportHandler sentErrorReportHandler = null;

        [MonoPInvokeCallback(typeof(NativeSentErrorReportDelegate))]
        public static void SentErrorReportNativeFunc(IntPtr report)
        {
            if (sentErrorReportHandler != null)
            {
                ErrorReport errorReport = CrashesInternal.GetErrorReportFromIntPtr(report);
                sentErrorReportHandler(errorReport);
            }
        }

        public static void SetSentErrorReportHandler(Crashes.SentErrorReportHandler handler)
        {
            sentErrorReportHandler = handler;
        }

#if ENABLE_IL2CPP
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
#endif
        public delegate void NativeFailedToSendErrorReportDelegate(IntPtr report);
        public static NativeFailedToSendErrorReportDelegate delegateFailedToSendErrorReport;
        private static Crashes.FailedToSendErrorReportHandler failedToSendErrorReportHandler = null;

        [MonoPInvokeCallback(typeof(NativeFailedToSendErrorReportDelegate))]
        public static void FailedToSendErrorReportNativeFunc(IntPtr report)
        {
            if (failedToSendErrorReportHandler != null)
            {
                ErrorReport errorReport = CrashesInternal.GetErrorReportFromIntPtr(report);
                failedToSendErrorReportHandler(errorReport);
            }
        }

        public static void SetFailedToSendErrorReportHandler(Crashes.FailedToSendErrorReportHandler handler)
        {
            failedToSendErrorReportHandler = handler;
        }

#region External

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_set_delegate();

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_delegate_set_should_process_error_report_delegate(NativeShouldProcessErrorReportDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_delegate_set_get_error_attachments_delegate(NativeGetErrorAttachmentsDelegate functionPtr);

        [DllImport("__Internal")]   
        private static extern IntPtr app_center_unity_crashes_get_error_attachment_log_text(string text, string fileName);

        [DllImport("__Internal")]   
        private static extern IntPtr app_center_unity_crashes_get_error_attachment_log_binary(byte[] data, int size, string fileName, string contentType);

        [DllImport("__Internal")]
        private static extern IntPtr app_center_unity_create_error_attachments_array(IntPtr errorAttachment0, IntPtr errorAttachment1);

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_delegate_set_sending_error_report_delegate(NativeSendingErrorReportDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_delegate_set_sent_error_report_delegate(NativeSentErrorReportDelegate functionPtr);

        [DllImport("__Internal")]
        private static extern void app_center_unity_crashes_delegate_set_failed_to_send_error_report_delegate(NativeFailedToSendErrorReportDelegate functionPtr);

#endregion
    
    }
}
#endif
