/*
	Team Spicy Bision
	Yan Zhang, Aidan Arrowwood, Drew Schneider
 */
#pragma strict

var spinx: int = 0;
var spiny: int = 0;
var spinz: int = 0;

function Start(){
	spiny = 2;

}
function Update () {
	transform.Rotate(spinx, spiny,spinz);
}