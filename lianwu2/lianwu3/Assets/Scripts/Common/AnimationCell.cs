using UnityEngine;
namespace LoveDance.Client.Common
{
	/// <summary>
	/// 动画扩展类，用于扩展UnityEngine.Animation类
	/// </summary>
	public class AnimationCell
	{
		private Animation mAni = null;	//动画

		public AnimationCell(GameObject targetObject)
		{
			if (targetObject == null)
			{
				Debug.LogError("AnimationCell error, add animation failed. TargetObject can not be null.");
			}
			else
			{
				mAni = targetObject.animation;
				if (mAni == null)
				{
					mAni = targetObject.AddComponent<Animation>();
				}
			}
		}

		public AnimationCell(Animation ani)
		{
			if (ani == null)
			{
				Debug.LogError("AnimationCell error, add animation failed. Animation can not be null.");
			}
			else
			{
				mAni = ani;
			}
		}

		public AnimationState this[string name]
		{
			get
			{
				if (mAni != null)
				{
					return mAni[name];
				}
				else
				{
					Debug.LogError("AnimationCell get animationState error, Animation can not be null.");
				}
				
				return null;
			}
		}

		public Animation Ani
		{
			get
			{
				return mAni;
			}
		}

		public AnimationClip clip
		{
			get
			{
				if (mAni == null)
				{
					Debug.LogError("AnimationCell Clip error, Animation can not be null.");
					return null;
				}
				else
				{
					return mAni.clip;
				}
			}
		}

		public void Destroy()
		{
			if (mAni != null)
			{
				GameObject.Destroy(mAni);
				mAni = null;
			}
		}

		public void DestroyClip(string clipName)
		{
			if (mAni != null)
			{
				AnimationClip clip = GetClip(clipName);
				if (clip != null)
				{
					mAni.Stop(clipName);
					mAni.RemoveClip(clip);

					Object.Destroy(clip);
				}
			}
		}

        public void DestroyClip(AniMoveState state)
        {
            DestroyClip(state.ToString());
        }

        public AnimationState AddClip(AnimationClip clip, AniMoveState state, WrapMode wrapMode, int layer, float weight, float speed)
        {
            return AddClip(clip, state.ToString(), wrapMode, layer, weight, speed);
        }		

		public AnimationState AddClip(AnimationClip clip, string clipName, WrapMode wrapMode, int layer, float weight, float speed)
		{
			if (mAni != null)
			{
				if (clip != null)
				{
					mAni.AddClip(clip, clipName);
					mAni[clipName].wrapMode = wrapMode;
					mAni[clipName].layer = layer;
					mAni[clipName].weight = weight;
					mAni[clipName].speed = speed;

					return mAni[clipName];
				}
				else
				{
					Debug.LogWarning("AddAnimation failed. AnimationClip can not be null." + clipName);
				}
			}
			else
			{
				Debug.LogError("AddAnimation failed. Animation can not be null.");
			}

			return null;
		}

        public AnimationState AddClip(AnimationClip clip, AniMoveState state, WrapMode wrapMode, int layer, float weight)
        {
            return AddClip(clip, state.ToString(), wrapMode, layer, weight);
        }

		public AnimationState AddClip(AnimationClip clip, string clipName, WrapMode wrapMode, int layer, float weight)
		{
			if (mAni != null)
			{
				if (clip != null)
				{
					mAni.AddClip(clip, clipName);
					mAni[clipName].wrapMode = wrapMode;
					mAni[clipName].layer = layer;
					mAni[clipName].weight = weight;

					return mAni[clipName];
				}
				else
				{
					Debug.LogWarning("AddAnimation failed. AnimationClip can not be null." + clipName);
				}
			}
			else
			{
				Debug.LogError("AddAnimation failed. Animation can not be null.");
			}

			return null;
		}

		public AnimationState AddClip(AnimationClip clip, string clipName, WrapMode wrapMode, int layer)
		{
			if (mAni != null)
			{
				if (clip != null)
				{
					mAni.AddClip(clip, clipName);
					mAni[clipName].wrapMode = wrapMode;
					mAni[clipName].layer = layer;

					return mAni[clipName];
				}
				else
				{
					Debug.LogWarning("AddAnimation failed. AnimationClip can not be null." + clipName);
				}
			}
			else
			{
				Debug.LogError("AddAnimation failed. Animation can not be null.");
			}

			return null;
		}

        public AnimationState AddClip(AnimationClip clip, string clipName, AnimStateParam param)
        {
            if (param.GetType() == typeof(ModeLayerParam))
            {
                ModeLayerParam theParam = (ModeLayerParam)param;
                return AddClip(clip, clipName, theParam.mode, theParam.layer);
            }
            else if (param.GetType() == typeof(ModeLayerWeightParam))
            {
                ModeLayerWeightParam theParam = (ModeLayerWeightParam)param;
                return AddClip(clip, clipName, theParam.mode, theParam.layer, theParam.weight);
            }
            else if (param.GetType() == typeof(ModeLayerTimeParam))
            {
                ModeLayerTimeParam theParam = (ModeLayerTimeParam)param;
                return AddClip(clip, clipName, theParam.mode, theParam.layer, theParam.time);
            }
            else
                return null;
        }

		public AnimationState AddClip(AnimationClip clip, string clipName, float time, WrapMode wrapMode, int layer)
		{
			if (mAni != null)
			{
				if (clip != null)
				{
					mAni.AddClip(clip, clipName);
					mAni[clipName].wrapMode = wrapMode;
					mAni[clipName].layer = layer;
					mAni[clipName].time = time;

					return mAni[clipName];
				}
				else
				{
					Debug.LogWarning("AddAnimation failed. AnimationClip can not be null." + clipName);
				}
			}
			else
			{
				Debug.LogError("AddAnimation failed. Animation can not be null.");
			}

			return null;
		}

		public AnimationState AddClip(AnimationClip clip, string clipName)
		{
			if (mAni != null)
			{
				if (clip != null)
				{
					mAni.AddClip(clip, clipName);

					return mAni[clipName];
				}
				else
				{
					Debug.LogWarning("AddAnimation failed. AnimationClip can not be null." + clipName);
				}
			}
			else
			{
				Debug.LogError("AddAnimation failed. Animation can not be null.");
			}

			return null;
		}

		public bool IsPlaying(string clipName)
		{
			if (mAni != null)
			{
				return mAni.IsPlaying(clipName);
			}
			else
			{
				Debug.LogWarning("IsAnimationPlaying failed. Animation can not be null." + clipName);
			}

			return false;
		}

		public AnimationClip GetClip(string clipName)
		{
			if (mAni != null)
			{
				return mAni.GetClip(clipName);
			}
			else
			{
				Debug.LogWarning("GetClip failed. Animation can not be null." + clipName);
			}

			return null;
		}

        public AnimationClip GetClip(AniMoveState state)
        {
            return GetClip(state.ToString());
        }

		public void CrossFade(string clipName)
		{
			if (mAni != null)
			{
				mAni.CrossFade(clipName);
			}
			else
			{
				Debug.LogWarning("CrossFadeAnimation failed. Animation can not be null." + clipName);
			}
		}

        public void CrossFade(AniMoveState state)
        {
            CrossFade(state.ToString());
        }

		public void CrossFade(string clipName, float fadeLength)
		{
			if (mAni != null)
			{
				mAni.CrossFade(clipName, fadeLength);
			}
			else
			{
				Debug.LogWarning("CrossFadeAnimation failed. Animation can not be null." + clipName);
			}
		}

		public void CrossFade(string clipName, float fadeLength, PlayMode playMode)
		{
			if (mAni != null)
			{
				mAni.CrossFade(clipName, fadeLength, playMode);
			}
			else
			{
				Debug.LogWarning("CrossFadeAnimation failed. Animation can not be null." + clipName);
			}
		}

        public void CrossFadeRewind(string clipName, float fadeLength, PlayMode playMode)
        {
            if (mAni != null)
            {
                if (GetClip(clipName) != null)
                {
                    Rewind(clipName);
                    CrossFade(clipName, fadeLength, playMode);
                }
            }
            else
            {
                Debug.LogWarning("CrossFadeAnimation failed. Animation can not be null." + clipName);
            }
        }

        public void CrossFade(AniMoveState state, float fadeLength, PlayMode playMode)
        {
            CrossFade(state.ToString(), fadeLength, playMode);
        }

        public void CrossFadeRewind(AniMoveState state, float fadeLength, PlayMode playMode)
        {
            if (GetClip(state) != null)
            {
                Rewind(state);
                CrossFade(state, fadeLength, playMode);
            }
        }

		public void CrossFadeQueued(string clipName)
		{
			if (mAni != null)
			{
				mAni.CrossFadeQueued(clipName);
			}
			else
			{
				Debug.LogWarning("CrossFadeQueuedAnimation failed. Animation can not be null." + clipName);
			}
		}

        public void CrossFadeQueued(AniMoveState state)
        {
            CrossFadeQueued(state.ToString());
        }

		public void CrossFadeQueued(string clipName, float fadeLength, QueueMode queue, PlayMode mode)
		{
			if (mAni != null)
			{
				mAni.CrossFadeQueued(clipName, fadeLength, queue, mode);
			}
			else
			{
				Debug.LogWarning("CrossFadeQueuedAnimation failed. Animation can not be null." + clipName);
			}
		}

		public void Rewind()
		{
			if (mAni != null)
			{
				mAni.Rewind();
			}
			else
			{
				Debug.LogWarning("RewindAnimation failed. Animation can not be null.");
			}
		}

		public void Rewind(string clipName)
		{
			if (mAni != null)
			{
				mAni.Rewind(clipName);
			}
			else
			{
				Debug.LogWarning("RewindAnimation failed. Animation can not be null." + clipName);
			}
		}

        public void Rewind(AniMoveState state)
        {
            Rewind(state.ToString());
        }

		public void Blend(string clipName, float targetWeight)
		{
			if (mAni != null)
			{
				mAni.Blend(clipName, targetWeight);
			}
			else
			{
				Debug.LogWarning("BlendAnimation failed. Animation can not be null." + clipName);
			}
		}

        public void Blend(AniMoveState state, float targetWeight)
        {
            Blend(state.ToString(), targetWeight);
        }


		public void Blend(string clipName, float targetWeight, float fadeLength)
		{
			if (mAni != null)
			{
				mAni.Blend(clipName, targetWeight, fadeLength);
			}
			else
			{
				Debug.LogWarning("BlendAnimation failed. Animation can not be null." + clipName);
			}
		}

		public void Play()
		{
			if (mAni != null)
			{
				mAni.Play();
			}
			else
			{
				Debug.LogWarning("PlayAnimation failed. Animation can not be null.");
			}
		}

		public void Play(string clipName)
		{
			if (mAni != null)
			{
				mAni.Play(clipName);
			}
			else
			{
				Debug.LogWarning("PlayAnimation failed. Animation can not be null." + clipName);
			}
		}

		public void Play(string clipName, PlayMode playMode)
		{
			if (mAni != null)
			{
				mAni.Play(clipName, playMode);
			}
			else
			{
				Debug.LogWarning("PlayAnimation failed. Animation can not be null." + clipName);
			}
		}

		public void Stop(string clipName)
		{
			if (mAni != null)
			{
				mAni.Stop(clipName);
			}
			else
			{
				Debug.LogWarning("StopAnimation failed. Animation can not be null." + clipName);
			}
		}

        public void Stop(AniMoveState state)
        {
            if (GetClip(state) != null)
                Stop(state.ToString());
        }

		public void Stop()
		{
			if (mAni != null)
			{
				mAni.Stop();
			}
			else
			{
				Debug.LogWarning("StopAnimation failed. Animation can not be null.");
			}
		}
	}

    public class AnimStateParam
    {
        public WrapMode mode = WrapMode.Default;
        public int layer = 0;

        public AnimStateParam() { }

        public AnimStateParam(WrapMode mode1, int layer1)
        {
            mode = mode1;
            layer = layer1;
        }
    }

    public class ModeLayerParam : AnimStateParam
    {
        public ModeLayerParam() { }

        public ModeLayerParam(WrapMode mode1, int layer1)
            : base(mode1, layer1) { }
    }

    public class ModeLayerWeightParam : ModeLayerParam
    {
        public int weight = 0;

        public ModeLayerWeightParam(WrapMode mode1, int layer1, int weight1)
            : base(mode1, layer1)
        {
            weight = weight1;
        }
    }

    public class ModeLayerTimeParam : ModeLayerParam
    {
        public float time = 0;

        public ModeLayerTimeParam() { }
        public ModeLayerTimeParam(WrapMode mode1, int layer1, float time1)
            : base(mode1, layer1)
        {
            time = time1;
        }
    }
}