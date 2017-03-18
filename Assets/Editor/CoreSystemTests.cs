using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class CoreSystemTests{
    
	[Test]
    [Category("CoreSystemTests")]
    public void CoreSystemStartTests() {
		//Arrange
		GameObject sut =  new GameObject();
        sut.AddComponent<CoreSystem>();

        GameObject.FindObjectOfType<CoreSystem>(); ; 
      
        //Assert
        //The object has a new name
        Assert.AreEqual(2, sut.GetComponent<CoreSystem>().coolDownTime);
	}
}
