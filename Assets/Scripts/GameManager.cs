using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {


    public float speed = 5.0F;
    public float ammoSpeed = 5.0F;
    public Transform playerTrans;
    public GameObject ammoPrefab;
    public GameObject enemyPrefab;
    public SpriteRenderer backGroundSpriteReanderer;
    public Resources resources;
    private List<GameObject> ammoList = new List<GameObject>();
    private bool shooted;

    void Start() {
        Application.targetFrameRate = 60;
        StartCoroutine(this.StartDance());
    }

    void FixedUpdate() {
        if (playerTrans != null) {
            Transform trans = this.playerTrans;
            bool right = Input.GetKey(KeyCode.RightArrow);
            bool left = Input.GetKey(KeyCode.LeftArrow);
            bool space = Input.GetKey(KeyCode.Space);
            float deltaSpeed = this.speed * Time.deltaTime;
            trans.Translate(right && left ? 0 : right ? deltaSpeed : left ? -deltaSpeed : 0, 0, 0);

            foreach (GameObject ammo in this.ammoList) {
                if (Vector3.Distance(trans.position, ammo.transform.position) >= 10) {
                    this.ammoList.Remove(ammo);
                    GameObject.Destroy(ammo);
                    continue;
                }

                ammo.transform.Translate(0, this.ammoSpeed * Time.deltaTime, 0);
            }

            if (space && !shooted) {
                this.ShootAmmo();
                this.resources.audioSource.PlayOneShot(this.resources.shootSound, 0.5F);
            }

            this.shooted = space;
        }
    }

    void ShootAmmo() {
        if (this.ammoPrefab != null) {
            GameObject obj = GameObject.Instantiate(this.ammoPrefab, this.playerTrans.position, Quaternion.identity);
            SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
            obj.transform.Translate(0, 1, 0);
            obj.name = "[PREFAB] AMMO";
            obj.tag = "AMMO";
            this.ammoList.Add(obj);
        }
    }

    private IEnumerator StartDance() {
        while (this.backGroundSpriteReanderer != null && this.backGroundSpriteReanderer.gameObject.activeSelf) {
            this.backGroundSpriteReanderer.flipX = !this.backGroundSpriteReanderer.flipX;
            yield return new WaitForSeconds(0.5F);
        }
    }
}
