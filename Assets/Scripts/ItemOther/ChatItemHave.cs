using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatItemHave : MonoBehaviour
{

    private GameObject have;

    // Start is called before the first frame update
    void Start()
    {
        have = transform.Find("Have").gameObject;

        have.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Facade.Instance.GetUnreadFriendMsg().ContainsKey(int.Parse(this.name)))
        {
            have.SetActive(true);
        }
    }
}
