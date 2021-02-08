using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
	public static Menu Instance;
	public string menuName;
	public bool open;


    private void Awake()
    {
		Instance = this;
    }
    public void Open()
	{
		open = true;
		gameObject.SetActive(true);
	}

	public void Close()
	{
		open = false;
		gameObject.SetActive(false);
	}
}