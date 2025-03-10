﻿/* 
QuickMute
Copyright 2017 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System;
using UnityEngine;

namespace QuickMute.Object {
    public class QVolume {
        bool mute;
        float master;

        public QVolume(float master, bool mute) {
            this.master = master;
            this.mute = mute;
            Apply();
        }

        public bool isMute {
            get {
                return mute;
            }
            set {
                mute = value;
                QSettings.Instance.Muted = mute;
                Apply();
            }
        }

        public float Master {
            get {
                return master;
            }
            set {
                if (isMute && value < 0.01) {
                    return;
                }
                bool refresh = System.Math.Abs(master - value) > 0.005 || isMute;
                master = Mathf.Clamp(value, 0, 1);
                if (refresh) {
                    isMute = false;
                    QuickMute.Instance.Refresh();
                    GameSettings.SaveSettings();
                } else {
                    Apply();
                }
            }
        }

        public void Apply() {
            Debug.Log("QuickMute.Apply, isMute: " + isMute + ", master: " + master);
            Debug.Log("StackTrace: " +  Environment.StackTrace);
            GameSettings.MASTER_VOLUME = isMute ? 0 : master;
        }

        public void Restore() {
            if (GameSettings.Ready) {
                GameSettings.MASTER_VOLUME = Mathf.Clamp(master, 0, 1);
                GameSettings.SaveSettings();
            } else {
                QSettings.Instance.Master = Mathf.Clamp(master, 0, 1);
                QSettings.Instance.Save();
            }
        }
    }
}
