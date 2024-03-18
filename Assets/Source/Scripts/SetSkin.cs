using UnityEngine;

namespace Source.Scripts
{
    public class SetSkin : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] _meshRenderers;

        public void Set(Material material)
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers) 
                meshRenderer.material = material;
        }
    }
}
