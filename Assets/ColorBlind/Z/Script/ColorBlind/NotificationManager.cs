using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
namespace ZTools {
    public class NotificationManager : Singleton<NotificationManager> {
        public Text notificationText;
        public CanvasGroup notificationGroup;

        public void DoNotification (string message) {
            notificationGroup.alpha = 1;
            notificationText.text = message;
        }
        public void DoNotificationAndFade (string message) {
            notificationGroup.alpha = 1;
            notificationText.text = message;
            DoNotificationFade ();
        }
        public void DoNotificationFade () {
            notificationGroup.DOPause ();
            notificationGroup.DOFade (0, 2);
        }
    }
}