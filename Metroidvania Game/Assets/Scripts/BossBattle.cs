using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    private CameraController theCam;
    public Transform camPosition;
    public float camSpeed;

    public int threshold1, threshold2; // �� ������� ���� ü��

    public float activeTime, fadeoutTime, inactiveTime; // ���� ������ ��Ÿ���ִ� �ð�, ������� �ð�, ������ִ� �ð�
    private float activeCounter, fadeCounter, inactiveCounter; // ������ �ð��� �ʽð� ����

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
                if (activeCounter > 0) // ��Ÿ���ִ� �ð��� �ʽð谡 ������ �ð��̶��
                {
                    activeCounter -= Time.deltaTime; // �ʽð谡 �پ��

                    if (activeCounter <= 0)  // ��Ÿ���ִ� �ð��� �ʽð谡 �����ٸ�
                    {
                        fadeCounter = fadeoutTime; // ������� �ʽð踦 ������ �ð��� ���߰�
                        anim.SetTrigger("vanish"); // ������ �ִϸ��̼��� ���
                    }

                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShots1;

                        Instantiate(bullet, shotPoint.position, shotPoint.rotation);
                    }

                }
                else if (fadeCounter > 0)   // ������� �ð��� �ʽð谡 ������ �ð��̶��
                {
                    fadeCounter -= Time.deltaTime;  // �ʽð谡 �پ��

                    if (fadeCounter <= 0)   // ������� �ð��� �ʽð谡 �����ٸ�
                    {
                        theBoss.gameObject.SetActive(false); // ������ ������� �ϰ�
                        inactiveCounter = inactiveTime;      // ������ִ� �ʽð踦 ������ �ð��� ���߰�
                    }
                }
                else if (inactiveCounter > 0) // ������ִ� �ð��� �ʽð谡 ������ �ð��̶��
                {
                    inactiveCounter -= Time.deltaTime;  // �ʽð谡 �پ��

                    if (inactiveCounter <= 0)   // ������ִ� �ð��� �ʽð谡 �����ٸ�
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position; // ������ ��ġ�� ���� ���� �������� �ٲ۴�
                        theBoss.gameObject.SetActive(true); // ������ ��Ÿ���� ��

                        activeCounter = activeTime; // ��Ÿ���ִ� �ʽð踦 ������ �ð��� ����

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
