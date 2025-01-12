using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public Vector2 moveVec;                     // ?´?™ ë°©í–¥ ë²¡í„°
    public Coroutine flinchCoroutine;           // ê²½ì§ ì½”ë£¨?‹´
    public Coroutine beAttackedCoroutine;       // ?”¼ê²? ?ƒ‰?ƒ ë³?ê²? ì½”ë£¨?‹´
    public GameObject hitTarget;                // ê³µê²© ?„±ê³?

    public bool isBeAttaked;                 // ?”¼ê²? : ?”¼ê²©ë¨
    public bool isFlinch;                   // ê²½ì§ : ?Š¤?Š¤ë¡? ???ì§ì¼ ?ˆ˜ ?—†?œ¼ë©? ê³µê²©?•  ?ˆ˜ ?—†?Œ
    public bool isSuperArmor;               // ê°•ì¸(?Šˆ?¼?•„ë¨?) : ê²½ì§ ?˜ì§? ?•Š?Š”?‹¤.
    public bool isInvincible;               // ë¬´ì  : ?”¼?•´??? ? ?˜ ê³µê²© ë¬´ì‹œ
    public bool isAttack;                   // ê³µê²© : ?Š¤?Š¤ë¡? ???ì§ì¼ ?ˆ˜ ?—†?œ¼ë©? ì¶”ê??ë¡? ê³µê²© ë¶ˆê??
    public bool isAttackReady = true;       // ê³µê²© ì¤?ë¹? : false?¼ ?‹œ ê³µê²©??? ?•  ?ˆ˜ ?—†?œ¼?‚˜ ?Š¤?Š¤ë¡? ?´?™??? ê°??Š¥

    public Transform fearTarget;            // °øÆ÷ ´ë»ó
    public Coroutine runCoroutine;          // µµ¸Á°¡±â ÄÚ·çÆ¾





}
