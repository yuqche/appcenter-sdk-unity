// Copyright (c) Microsoft Corporation. All rights reserved.
//
// Licensed under the MIT license.

#import "MobileCenterStarter.h"
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@import MobileCenter;

#ifdef MOBILE_CENTER_UNITY_USE_PUSH
@import MobileCenterPush;
#import "../Push/PushDelegate.h"
#endif

#ifdef MOBILE_CENTER_UNITY_USE_ANALYTICS
@import MobileCenterAnalytics;
#endif

#ifdef MOBILE_CENTER_UNITY_USE_DISTRIBUTE
@import MobileCenterDistribute;
#import "../Distribute/DistributeDelegate.h"
#endif

@implementation MobileCenterStarter

static NSString *const kMSAppSecret = @"mobile-center-app-secret";
static NSString *const kMSCustomLogUrl = @"custom-log-url";
static const int kMSLogLevel = 0/*LOG_LEVEL*/;

+ (void)load {
  [[NSNotificationCenter defaultCenter] addObserver:self
                                           selector:@selector(startMobileCenter)
                                               name:UIApplicationDidFinishLaunchingNotification
                                             object:nil];
}

+ (void)startMobileCenter {
  NSMutableArray<Class>* classes = [[NSMutableArray alloc] init];

#ifdef MOBILE_CENTER_UNITY_USE_PUSH
  [MSPush setDelegate:[UnityPushDelegate sharedInstance]];
  [classes addObject:MSPush.class];
#endif

#ifdef MOBILE_CENTER_UNITY_USE_ANALYTICS
  [classes addObject:MSAnalytics.class];
#endif

#ifdef MOBILE_CENTER_UNITY_USE_DISTRIBUTE
  [MSDistribute setDelegate:[UnityDistributeDelegate sharedInstance]];
  [classes addObject:MSDistribute.class];
#endif

  [MSMobileCenter setLogLevel:(MSLogLevel)kMSLogLevel];

#ifdef MOBILE_CENTER_UNITY_USE_CUSTOM_LOG_URL
  [MSMobileCenter setLogUrl:kMSCustomLogUrl];
#endif

  [MSMobileCenter start:kMSAppSecret withServices:classes];
}

@end
