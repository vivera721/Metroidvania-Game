using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController theCam;
    public Transform camPosition;
    public float camSpeed;

    public int threshold1, threshold2; // 각 페이즈당 돌입 체력

    public float activeTime, fadeoutTime, inactiveTime; // 각각 정해진 나타나있는 시간, 사라지는 시간, 사라져있는 시간
    private float activeCounter, fadeCounter, inactiveCounter; // 각각의 시간의 초시계 개념

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;

    public Animator anim;

    public Transform theBoss;

    public float timeBetweenShots1, timeBetweenShots2;
    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;

    public GameObject winObjects;

    private bool battleEnded;

    void Start()
    {
        theCam = FindObjectOfType<CameraController>();
        theCam.enabled = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShots1;
    }

    void Update()
    {
        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosition.position, camSpeed * Time.deltaTime);

        if (!battleEnded)
        {
            if (BossHealthController.instance.currentHealth > threshold1)
            {
                if (activeCounter > 0) // 나타나있는 시간의 초시계가 정해진 시간이라면
                {
                    activeCounter -= Time.deltaTime; // 초시계가 줄어듦

                    if (activeCounter <= 0)  // 나타나있는 시간의 초시계가 끝났다면
                    {
                        fadeCounter = fadeoutTime; // 사라지는 초시계를 정해진 시간에 맞추고
                        anim.SetTrigger("vanish"); // 정해진 애니메이션을 출력
                    }

                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShots1;

                        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
                    }

                }
                else if (fadeCounter > 0)   // 사라지는 시간의 초시계가 정해진 시간이라면
                {
                    fadeCounter -= Time.deltaTime;  // 초시계가 줄어듦

                    if (fadeCounter <= 0)   // 사라지는 시간의 초시계가 끝났다면
                    {
                        theBoss.gameObject.SetActive(false); // 보스를 사라지게 하고
                        inactiveCounter = inactiveTime;      // 사라져있는 초시계를 정해진 시간에 맞추고
                    }
                }
                else if (inactiveCounter > 0) // 사라져있는 시간의 초시계가 정해진 시간이라면
                {
                    inactiveCounter -= Time.deltaTime;  // 초시계가 줄어듦

                    if (inactiveCounter <= 0)   // 사라져있는 시간의 초시계가 끝났다면
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position; // 보스의 위치를 랜덤 스폰 지점으로 바꾼다
                        theBoss.gameObject.SetActive(true); // 보스를 나타나게 함

                        activeCounter = activeTime; // 나타나있는 초시계를 정해진 시간에 맞춤

                        shotCounter = timeBetweenShots1;
                    }
                }
            }
            else
            {
                if (targetPoint == null)
                {
                    targetPoint = theBoss;
                    fadeCounter = fadeoutTime;
                    anim.SetTrigger("vanish");
                }
                else
                {
                    if (Vector3.Distance(theBoss.position, targetPoint.position) > .02f)
                    {
                        theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);

                        if (Vector3.Distance(theBoss.position, targetPoint.position) <= .02f)
                        {
                            fadeCounter = fadeoutTime;
                            anim.SetTrigger("vanish");
                        }

                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            if (PlayerHealthController.instance.currentHealth > threshold2)
                            {
                                shotCounter = timeBetweenShots1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShots2;
                            }

                            Instantiate(bullet, shotPoint.position, shotPoint.rotation);
                        }
                    }
                    else if (fadeCounter > 0)
                    {
                        fadeCounter -= Time.deltaTime;

                        if (fadeCounter <= 0)
                        {
                            theBoss.gameObject.SetActive(false);
                            inactiveCounter = inactiveTime;
                        }
                    }
                    else if (inactiveCounter > 0)
                    {
                        inactiveCounter -= Time.deltaTime;

                        if (inactiveCounter <= 0)
                        {
                            theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                            int whileBreaker = 0;

                            while (targetPoint.position == theBoss.position && whileBreaker < 100)
                            {
                                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                                whileBreaker++;
                            }


                            theBoss.gameObject.SetActive(true);


                            shotCounter -= Time.deltaTime;
                            if (shotCounter <= 0)
                            {
                                if (PlayerHealthController.instance.currentHealth > threshold2)
                                {
                                    shotCounter = timeBetweenShots1;
                                }
                                else
                                {
                                    shotCounter = timeBetweenShots2;
                                }


                            }
                        }
                    }
                }
            }
        }
        else
        {
            fadeCounter -= Time.deltaTime;
            if (fadeCounter < 0)
            {
                if (winObjects != null)
                {
                    winObjects.SetActive(true);

                    winObjects.transform.SetParent(null);
                }

                theCam.enabled = true;

                gameObject.SetActive(false);
            }
        }

    }

    public void EndBattle()
    {
        battleEnded = true;

        fadeCounter = fadeoutTime;
        anim.SetTrigger("vanish");
        theBoss.GetComponent<Collider2D>().enabled = false;

        BossBullet[] bullets = FindObjectsOfType<BossBullet>();
        if (bullets.Length > 0)
        {
            foreach (BossBullet bb in bullets)
            {
                Destroy(bb.gameObject);
            }
        }
    }

}
