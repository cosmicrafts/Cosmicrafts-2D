using JoshH.Extensions;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Author: Josh H.
/// Support: assetstore.joshh@gmail.com
/// </summary>

namespace JoshH.UI
{
    [AddComponentMenu("UI/Effects/UI Gradient")]
    [RequireComponent(typeof(RectTransform))]
    public class UIGradient : BaseMeshEffect
    {
        [Tooltip("How the gradient color will be blended with the graphics color.")]
        [SerializeField] private UIGradientBlendMode blendMode;

        [SerializeField] [Range(0, 1)] private float intensity = 1f;

        [SerializeField] private UIGradientType gradientType;

        //Linear Colors
        [SerializeField] private Color linearColor1 = Color.yellow;
        [SerializeField] private Color linearColor2 = Color.red;

        //Corner Colors
        [SerializeField] private Color cornerColorUpperLeft = Color.red;
        [SerializeField] private Color cornerColorUpperRight = Color.yellow;
        [SerializeField] private Color cornerColorLowerRight = Color.green;
        [SerializeField] private Color cornerColorLowerLeft = Color.blue;

        [SerializeField] [Range(0, 360)] private float angle;

        private RectTransform _rectTransform;

        protected RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = transform as RectTransform;
                }
                return _rectTransform;
            }
        }

        public UIGradientBlendMode BlendMode
        {
            get
            {
                return blendMode;
            }

            set
            {
                blendMode = value;
            }
        }

        public float Intensity
        {
            get
            {
                return intensity;
            }

            set
            {
                intensity = Mathf.Clamp01(value);
            }
        }

        public UIGradientType GradientType
        {
            get
            {
                return gradientType;
            }

            set
            {
                gradientType = value;
            }
        }

        public Color LinearColor1
        {
            get
            {
                return linearColor1;
            }

            set
            {
                linearColor1 = value;
            }
        }

        public Color LinearColor2
        {
            get
            {
                return linearColor2;
            }

            set
            {
                linearColor2 = value;
            }
        }

        public Color CornerColorUpperLeft
        {
            get
            {
                return cornerColorUpperLeft;
            }

            set
            {
                cornerColorUpperLeft = value;
            }
        }

        public Color CornerColorUpperRight
        {
            get
            {
                return cornerColorUpperRight;
            }

            set
            {
                cornerColorUpperRight = value;
            }
        }

        public Color CornerColorLowerRight
        {
            get
            {
                return cornerColorLowerRight;
            }

            set
            {
                cornerColorLowerRight = value;
            }
        }

        public Color CornerColorLowerLeft
        {
            get
            {
                return cornerColorLowerLeft;
            }

            set
            {
                cornerColorLowerLeft = value;
            }
        }

        public float Angle
        {
            get
            {
                return angle;
            }

            set
            {
                if (value < 0)
                {
                    angle = (value % 360) + 360;
                }
                else
                {
                    angle = value % 360;
                }
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (this.enabled)
            {
                UIVertex vert = new UIVertex();

                for (int i = 0; i < vh.currentVertCount; i++)
                {
                    vh.PopulateUIVertex(ref vert, i);

#if UNITY_2018_1_OR_NEWER
                    Vector2 normalizedPosition = ((Vector2)vert.position - rectTransform.rect.min) / (rectTransform.rect.max - rectTransform.rect.min);                
#else
                    Vector2 size = rectTransform.rect.max - rectTransform.rect.min;
                    Vector2 normalizedPosition = Vector2.Scale((Vector2)vert.position - rectTransform.rect.min, new Vector2(1f / size.x, 1f / size.y));
#endif

                    normalizedPosition = RotateNormalizedPosition(normalizedPosition, this.angle);

                    //get color with selected gradient type
                    Color gradientColor = Color.black;
                    if (gradientType == UIGradientType.Linear)
                    {
                        gradientColor = GetColorInGradient(linearColor1, linearColor1, linearColor2, linearColor2, normalizedPosition);
                    }
                    else if (gradientType == UIGradientType.Corner)
                    {
                        gradientColor = GetColorInGradient(cornerColorUpperLeft, cornerColorUpperRight, cornerColorLowerRight, cornerColorLowerLeft, normalizedPosition);
                    }
                    vert.color = BlendColor(vert.color, gradientColor, blendMode, intensity);
                    vh.SetUIVertex(vert, i);
                }
            }
        }

        private Color BlendColor(Color c1, Color c2, UIGradientBlendMode mode, float intensity)
        {
            if (mode == UIGradientBlendMode.Override)
            {
                return Color.Lerp(c1, c2, intensity);
            }
            else if (mode == UIGradientBlendMode.Multiply)
            {
                return Color.Lerp(c1, c1 * c2, intensity);
            }
            else
            {
                Debug.LogErrorFormat("Mode is not supported: {0}", mode);
                return c1;
            }
        }

        /// <summary>
        /// Rotates a position in with coordinates in [0,1]
        /// </summary>
        /// <param name="normalizedPosition">Point to rotate</param>
        /// <param name="angle">Angle to rotate in degrees</param>
        /// <returns>Rotated point</returns>
        private Vector2 RotateNormalizedPosition(Vector2 normalizedPosition, float angle)
        {
            float a = Mathf.Deg2Rad * (angle < 0 ? (angle % 90 + 90) : (angle % 90));
            float scale = Mathf.Sin(a) + Mathf.Cos(a);

            return (normalizedPosition - Vector2.one * 0.5f).Rotate(angle) / scale + Vector2.one * 0.5f;
        }

        /// <summary>
        /// Sets vertices of the referenced Graphic dirty. This triggers a new mesh generation and modification.
        /// </summary>
        public void ForceUpdateGraphic()
        {
            if (this.graphic != null)
            {
                this.graphic.SetVerticesDirty();
            }
        }

        /// <summary>
        /// Calculates color interpolated between 4 corners.
        /// </summary>
        /// <param name="ul">upper left (0,1)</param>
        /// <param name="ur">upper right (1,1)</param>
        /// <param name="lr">lower right (1,0)</param>
        /// <param name="ll">lower left (0,0)</param>
        /// <param name="normalizedPosition">position (x,y) in [0,1]</param>
        /// <returns>interpolated color</returns>
        private Color GetColorInGradient(Color ul, Color ur, Color lr, Color ll, Vector2 normalizedPosition)
        {
            return Color.Lerp(Color.Lerp(ll, lr, normalizedPosition.x), Color.Lerp(ul, ur, normalizedPosition.x), normalizedPosition.y); ;
        }

        public enum UIGradientBlendMode
        {
            Override,
            Multiply
        }

        public enum UIGradientType
        {
            Linear,
            Corner
        }
    }
}