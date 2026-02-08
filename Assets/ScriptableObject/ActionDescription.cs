using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInteractableDescription", menuName = "InteractableDescription")]

/// 상호작용 설명의 데이터
public class ActionDescription : ScriptableObject
{
    [field: SerializeField] public string m_Description { get; private set; }
}
