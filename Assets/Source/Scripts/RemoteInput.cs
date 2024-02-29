using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace Source.Scripts
{
    public class RemoteInput : MonoBehaviour
    {
        public void SetPosition(Vector3 position) => 
            transform.position = position;

        public void OnChange(List<DataChange> changes)
        {
            Vector3 position = transform.position;
            foreach (DataChange dataChange in changes)
            {
                switch (dataChange.Field)
                {
                    case "x":
                        position.x = (float) dataChange.Value;
                        break;
                    case "y":
                        position.z = (float) dataChange.Value;
                        break;
                    default:
                        Debug.LogWarning($"Field {dataChange.Field} is not processed.");
                        break;
                }
            }

            transform.position = position;
        }
    }
}