using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class CoreSystemTests{
    
	[Test]
    [Category("CoreSystemTests")]
    public void CoreSystemStartTest() {
		//Arrange
		CoreSystem sut = new CoreSystem();
        		
		//Assert
		//The object has a new name
		Assert.AreEqual(2, sut.coolDownTime);
	}
}
