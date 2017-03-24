using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class EventBusManagerTests
{
    
	[Test]
    [Category("EventBusManagerTests")]
    public void EventBusManagerFindTests() {
		//Arrange
		GameObject sut =  new GameObject();
        sut.AddComponent<EventBusManager>();

        GameObject.FindObjectOfType<EventBusManager>(); ; 
      
        //Assert
        //The object has a new name
        Assert.AreEqual(2, sut.GetComponent<EventBusManager>().coolDownTime);
	}
}
