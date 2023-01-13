using UnityEngine;


namespace AmazingAssets.DynamicRadialMasks
{
    [System.Serializable]
    public class DRMLiveObject
    {

#if UNITY_EDITOR
        [HideInInspector] public bool displayAllProperties = true;
        [HideInInspector] public DynamicRadialMasks.Enum.MaskShape maskShape;        
#endif

        public Vector3 position;

        public Vector2 lifeLength = Vector2.one;    

        public DRMLiveProperty radius = new DRMLiveProperty();
        public DRMLiveProperty intensity = new DRMLiveProperty();
        public DRMLiveProperty noise = new DRMLiveProperty();
        public DRMLiveProperty edgeSize = new DRMLiveProperty();
        public DRMLiveProperty ringCount = new DRMLiveProperty();
        public DRMLiveProperty phaseSpeed = new DRMLiveProperty();
        public DRMLiveProperty frequency = new DRMLiveProperty();
        public DRMLiveProperty smooth = new DRMLiveProperty();


        float currentLifeLength;



        public DRMLiveObject()
        {
            Reset();
        }

        public DRMLiveObject(DRMLiveObject obj, Vector3 position)
        {
            this.position = position;

            currentLifeLength = Random.Range(obj.lifeLength.x, obj.lifeLength.y);

            radius = new DRMLiveProperty(obj.radius, currentLifeLength);
            intensity = new DRMLiveProperty(obj.intensity, currentLifeLength);
            noise = new DRMLiveProperty(obj.noise, currentLifeLength);
            edgeSize = new DRMLiveProperty(obj.edgeSize, currentLifeLength);
            ringCount = new DRMLiveProperty(obj.ringCount, currentLifeLength);
            phaseSpeed = new DRMLiveProperty(obj.phaseSpeed, currentLifeLength);
            frequency = new DRMLiveProperty(obj.frequency, currentLifeLength);
            smooth = new DRMLiveProperty(obj.smooth, currentLifeLength);
        }

        public void Reset()
        {
            radius = new DRMLiveProperty();
            intensity = new DRMLiveProperty();
            noise = new DRMLiveProperty(DRMLiveProperty.Enum.AnimationType.Constant, Vector2.zero, Vector2.zero, null);
            edgeSize = new DRMLiveProperty();
            ringCount = new DRMLiveProperty();
            phaseSpeed = new DRMLiveProperty();
            frequency = new DRMLiveProperty();
            smooth = new DRMLiveProperty();

            lifeLength = Vector2.one;
        }

        public bool Animate()
        {            
            radius.Animate(Time.deltaTime);
            intensity.Animate(Time.deltaTime);
            noise.Animate(Time.deltaTime);
            edgeSize.Animate(Time.deltaTime);
            phaseSpeed.Animate(Time.deltaTime);
            ringCount.Animate(Time.deltaTime);
            frequency.Animate(Time.deltaTime);
            smooth.Animate(Time.deltaTime);


            currentLifeLength -= Time.deltaTime;

            return (currentLifeLength > 0) ? true : false;   //Can be animation be continued in the next frame?
        }
    }
}