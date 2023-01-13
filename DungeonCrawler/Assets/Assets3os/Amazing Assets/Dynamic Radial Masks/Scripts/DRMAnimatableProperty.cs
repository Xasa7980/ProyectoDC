using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    [System.Serializable]
    public class DRMLiveProperty
    {
        public class Enum
        {
            public enum AnimationType { Constant, ConstantRange, Lerp, LerpRange, Curve };
        }


        public Enum.AnimationType evolutionType;
        public Vector2 startValue;      //x - min, y - max
        public Vector2 endValue;        //x - min, y - max
        public AnimationCurve curve;    //Time range always is in the range [0, 1]

        public float currentValue;      

        float lifeLength;
        float currentAge;
        float evolutionValueStart;
        float evolutionValueEnd;


        public DRMLiveProperty()
        {
            this.evolutionType = Enum.AnimationType.Constant;
            this.startValue = Vector2.one;
            this.endValue = Vector2.one * 2;
            this.curve = AnimationCurve.Linear(0, 1, 0, 1);
            this.lifeLength = 1;

            Initialize();
        }

        public DRMLiveProperty(DRMLiveProperty property, float lifeLength)
        {
            this.evolutionType = property.evolutionType;
            this.startValue = property.startValue;
            this.endValue = property.endValue;
            this.curve = property.curve;
            this.lifeLength = lifeLength;

            Initialize();
        }

        public DRMLiveProperty(Enum.AnimationType evolution, Vector2 start, Vector2 end, AnimationCurve curve)
        {
            this.evolutionType = evolution;
            this.startValue = start;
            this.endValue = end;
            this.curve = curve;

            Initialize();
        }

        public void Scale(float value)
        {
            startValue *= value;
            endValue *= value;

            if (curve != null)
            {
                for (int i = 0; i < curve.length; i++)
                {
                    curve.keys[i].value *= value;
                }
            }
        }

        void Initialize()
        {
            currentAge = 0;

            switch (evolutionType)
            {
                case Enum.AnimationType.Constant:
                    evolutionValueStart = evolutionValueEnd = startValue.x;
                    break;

                case Enum.AnimationType.ConstantRange:
                    evolutionValueStart = evolutionValueEnd = Random.Range(startValue.x, startValue.y);
                    break;


                case Enum.AnimationType.Lerp:
                    evolutionValueStart = startValue.x;
                    evolutionValueEnd = endValue.x;
                    break;

                case Enum.AnimationType.LerpRange:
                    evolutionValueStart = Random.Range(startValue.x, startValue.y);
                    evolutionValueEnd = Random.Range(endValue.x, endValue.y);
                    break;
            }
        }


        public void Animate(float deltaTime) 
        {
            switch (evolutionType)
            {
                case Enum.AnimationType.Constant:
                case Enum.AnimationType.ConstantRange:
                    currentValue = evolutionValueStart;
                    break;


                case Enum.AnimationType.Lerp:
                case Enum.AnimationType.LerpRange:
                    {
                        currentAge += deltaTime;
                        float ageRatio = Mathf.Clamp01(currentAge / lifeLength);

                        currentValue = Mathf.Lerp(evolutionValueStart, evolutionValueEnd, ageRatio);
                    }
                    break;


                case Enum.AnimationType.Curve:
                    {
                        currentAge += deltaTime;
                        float ageRatio = Mathf.Clamp01(currentAge / lifeLength);

                        currentValue = curve == null ? 0 : curve.Evaluate(ageRatio);
                    }
                    break;

                default:
                    currentValue = 0;
                    break;
            }
        }
    }
}