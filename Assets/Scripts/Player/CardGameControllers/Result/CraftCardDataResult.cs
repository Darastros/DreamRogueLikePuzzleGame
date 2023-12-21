using UnityEngine;

namespace CardGame
{
    [CreateAssetMenu(menuName = "Result/data")]
    public class CraftCardDataResult : CraftCardResult
    {
        [SerializeField] public int lifePoints = 0;
        [SerializeField] public bool artifact = false;
        [SerializeField] public bool portalPart = false;
        
        public override void Apply(Vector3 _position)
        {
            base.Apply(_position);
            if (lifePoints != 0) PlayerDataManager.life += lifePoints;
            if (artifact) ++PlayerDataManager.artifact;
            if (portalPart) PlayerDataManager.cardGameKeyPart = true;
        }
    }
}