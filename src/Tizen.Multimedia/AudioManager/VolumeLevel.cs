﻿ /*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace Tizen.Multimedia
{
    internal static class VolumeLevelLog
    {
        internal const string Tag = "Tizen.Multimedia.VolumeLevel";
    }

    /// <summary>
    /// This is a indexer class which is used to get/set the volume level
    /// specified for a particular sound type.
    /// </summary>
    public class VolumeLevel
    {
        public int this [AudioVolumeType type] {
            get {
                if(type == AudioVolumeType.None)
                    throw new ArgumentException("Wrong Audio volume type. Cannot get volume level for AudioVolumeType.None");
                int volume;
                int ret = Interop.AudioVolume.GetVolume(type, out volume);
                if(ret != 0) {
                    Tizen.Log.Info(VolumeLevelLog.Tag, "Get Level Error: " + (AudioManagerError)ret);
                    return -1;
                }
                return volume;
            }
            set {
                if(type == AudioVolumeType.None)
                    throw new ArgumentException("Wrong Audio volume type. Cannot set volume level for AudioVolumeType.None");
                int ret = Interop.AudioVolume.SetVolume(type, value);
                if(ret != 0) {
                    Tizen.Log.Info(VolumeLevelLog.Tag, "Set Level Error: " + (AudioManagerError)ret);
                    AudioManagerErrorFactory.CheckAndThrowException(ret, "Unable to set level");
                }
            }
        }
    }
}