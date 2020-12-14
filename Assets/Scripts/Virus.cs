using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to find out if Ghost hits a wall
using System;

public class Virus : MonoBehaviour
{

	public float speed = 4f; // prędkość danego wirusa
	private Rigidbody2D rb; // do poruszania danym wirusem

	// grafiki przedstawiające wirusa patrzącego w cztery kierunki
	public Sprite lookLeftSprite;
	public Sprite lookRightSprite;
	public Sprite lookUpSprite;
	public Sprite lookDownSprite;

	// punkty do których kolejno będzie poruszał się wirus
	Vector2[] destinations = new Vector2[]{
		new Vector2( 1, 29 ),
		new Vector2( 26, 29 ),
		new Vector2( 26, 1 ),
		new Vector2( 1, 1 ),
		new Vector2( 6, 16 )
	};
	
	public int destinationIndex; // indeks wskazujący do którego punktu teraz ma się poruszać wirus
	Vector2 moveVect; // kierunek w stronę punktu do którego teraz ma się poruszać wirus
	public SpriteRenderer sr; 
	public bool isVirusBlue = false; // status wirusa
	public Sprite blueVirus; // grafika niebieskiego wirusa

	public float startWaitTime = 0; // czas po którym wirus zacznie się poruszać na planszy
	public float waitTimeAfterEaten = 4.0f; // czas po którym wirus pojawi się ponownie po zjedzeniu

	// współrzędne wirusa na planszy
	public float cellXPos = 0; 
	public float cellYPos = 0;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>(); // pobranie referencji do rigidbody wirusa
		sr = gameObject.GetComponent<SpriteRenderer>(); // pobranie komponentu SpriteRenderer z obiektu
	}

	void Start()
	{
		Invoke("startMoving", startWaitTime); // rozpoczęczie ruchu po czasie startWaitTime
	}

	void startMoving()
    {
		// ustawienie wirusa na środku planszy
		transform.position = new Vector2(13.5f, 18.5f); 
		// pobranie współrzędnej x punktu do którego porusza się wirus
		float xDest = destinations[destinationIndex].x;

		if (transform.position.x > xDest) // jeśli wirus ma iść w lewo
			rb.velocity = new Vector2(-1, 0) * speed; // nadajemy wirusowi pęd w lewo
		else // jeśli wirus ma iść w prawo
			rb.velocity = new Vector2(1, 0) * speed; // nadajemy wirusowi pęd w prawo
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		// jeśli wirus napotka TurningPoint
		if (col.gameObject.tag == "Point")
		{
			// wyszukanie kolejnego punktu w którym ma się poruszać wirus
			moveVect = getNewDirection(col.transform.position);

			// wyśrodkowanie wirusa do środka kratki
			transform.position = new Vector2(
				(int)col.transform.position.x + .5f, 
				(int)col.transform.position.y + .5f);

			// wartość 2 oznacza error, x i y wektora przyjmuje tylko wartości -1, 0 i 1
			if (moveVect.x != 2)
			{
				rb.velocity = moveVect * speed; // zmiana kierunku poruszania się wirusa

                // jeśli wirus nie jest niebieski
                if (!isVirusBlue)
                {
					// ustawienie kierunku patrzenia wirusa zgodnie z jego kierunkiem ruchu
					if (moveVect == Vector2.right) sr.sprite = lookRightSprite;
					else if (moveVect == Vector2.left) sr.sprite = lookLeftSprite;
					else if (moveVect == Vector2.up) sr.sprite = lookUpSprite;
					else if (moveVect == Vector2.down) sr.sprite = lookDownSprite;
				}
			}
		}

		// przejście wirusa przez teleport
		if(transform.position.x < 2 && transform.position.y == 15.5) // lewy portal
        {
			transform.position = new Vector2(24.5f, 15.5f); // zmiana jego położenia
			Vector2 virusMoveVect = new Vector2(-1, 0); // zmiana kierunku w którym patrzy
			rb.velocity = virusMoveVect * speed; // nadanie prędkości wirusowi
        } 
		else if (transform.position.x > 25 && transform.position.y == 15.5) // prawy portal
		{
			transform.position = new Vector2(2f, 15.5f);
			Vector2 virusMoveVect = new Vector2(1, 0);
			rb.velocity = virusMoveVect * speed;
		}
	}

	GameObject pacmanGO = null;

	public void ResetVirusAfterEaten(GameObject pacman)
    {
		transform.position = new Vector2(cellXPos, cellYPos); // przesunięcie wirusa do klatki												 
		rb.velocity = Vector2.zero; // zatrzymanie wirusa					  
		pacmanGO = pacman; // od teraz wirus chce złąpać Pacmana
		Invoke("startMoving", waitTimeAfterEaten); // wypuść wirusa po waitTImeAfterEaten
	}

	Vector2 getNewDirection(Vector2 pointVect)
	{
		// pobranie i zaokrąglenie pozycji wirusa co wartości całkowitych
		float xPos = (float)Math.Floor(Convert.ToDouble(transform.position.x));
		float yPos = (float)Math.Floor(Convert.ToDouble(transform.position.y));

		// zaokrąglenie pozycji TurningPoint na którym znajuje się wirus
		pointVect.x = (float)Math.Floor(Convert.ToDouble(pointVect.x));
		pointVect.y = (float)Math.Floor(Convert.ToDouble(pointVect.y));

		// gdzie ma iść wirus
		Vector2 dest = destinations[destinationIndex];

		// sprawdzamy czy wirus doszedł już tam gdzie chciał dojść
		if (((pointVect.x + 1) == dest.x) && ((pointVect.y + 1) == dest.y))
			destinationIndex = (destinationIndex == 4) ? 0 : destinationIndex + 1; // wybranie nowego celu

		dest = destinations[destinationIndex]; // pobranie celu z tablicy

		// jeśli wirus ma gonić gracza
		if (pacmanGO != null)
			dest = pacmanGO.transform.position; // gracz jest celem
		
		Vector2 newDir = new Vector2(2, 0); // wektor jaki zwracamy, 2 oznacza że wystąpił błąd i nie znaleśliśmy nowego wektora
		Vector2 prevDir = rb.velocity.normalized; // zapamiętujemy poprzedni kierunek ruchu
		Vector2 oppPrevDir = prevDir * -1; // przeciwieństwo poprzedniego kierunku

		// cztery kierunki, wektory te uproszczą kod
		Vector2 goRight = new Vector2(1, 0);
		Vector2 goLeft = new Vector2(-1, 0);
		Vector2 goUp = new Vector2(0, 1);
		Vector2 goDown = new Vector2(0, -1);

		// odległość od celu
		float destXDist = dest.x - xPos;
		float destYDist = dest.y - yPos;

		// GÓRNY LEWY
		if (destYDist > 0 && destXDist < 0)
		{
			// ominięcie portalu
			if (pointVect.x == 5 && pointVect.y == 15)
			{
				if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			}
			else if (destYDist > destXDist)
			{
				if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
				else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
			}
			else if (destYDist < destXDist)
			{
				if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
				else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
			}
		}

		// GÓRNY PRAWY
		if (destYDist > 0 && destXDist > 0)
		{
			if (destYDist > destXDist)
			{
				if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
				else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
			}
			else if (destYDist < destXDist)
			{
				if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
				else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
			}
		}

		// DOLNY LEWY
		if (destYDist < 0 && destXDist > 0)
		{
			if (destYDist > destXDist)
			{
				if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
				else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			}
			else if (destYDist < destXDist)
			{
				if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
				else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			}
		}

		// DOLNY PRAWY
		if (destYDist < 0 && destXDist < 0)
		{
			if (destYDist > destXDist)
			{
				if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
				else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			}
			else if (destYDist < destXDist)
			{
				if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
				else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
				else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
				else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			}
		}

		// współżędne y wirusa i celu są takie same i wirus chce iść w prawo
		if ((int)(dest.y) == (int)(yPos) && destXDist > 0)
		{
			if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
			else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
			else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
		}

		// współżędne y wirusa i celu są takie same i wirus chce iść w lewo
		if ((int)(dest.y) == (int)(yPos) && destXDist < 0)
		{
			if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
			else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
			else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
		}

		// współżędne x wirusa i celus są takie same i wirus chce iść do góry
		if ((int)(dest.x) == (int)(xPos) && destYDist > 0)
		{
			if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
			else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
			else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
			else if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
		}

		// współżądne x wirusa i celu są takie same i wirus chce iść w dół
		if ((int)(dest.x) == (int)(xPos) && destYDist < 0)
		{
			if (canIMoveInDirection(goDown, pointVect) && goDown != oppPrevDir) newDir = goDown;
			else if (canIMoveInDirection(goRight, pointVect) && goRight != oppPrevDir) newDir = goRight;
			else if (canIMoveInDirection(goLeft, pointVect) && goLeft != oppPrevDir) newDir = goLeft;
			else if (canIMoveInDirection(goUp, pointVect) && goUp != oppPrevDir) newDir = goUp;
		}

		return newDir; // zwracamy obrany kierunek
	}

	// Gets a chosen direction and searches for it in the array
	// that holds references to all the pivot points
	bool canIMoveInDirection(Vector2 dir, Vector2 pointVect)
	{
		// pobranie współrzędnych Obiektu TurningPoint o takich samych współrzędnych co wirus
		Transform point = GameObject.Find("GBGrid").GetComponent<GameScene>().gBPoints[(int)pointVect.x, (int)pointVect.y];

		// jeśli pobrano współrzędne
		if (point != null)
		{
			// pobranie Obiektu TurningPoint
			GameObject pointGO = point.gameObject;
			// pobranie pobliskich obiektów TurningPoint
			Vector2[] vectToNextPoint = pointGO.GetComponent<TurningPoint>().vectToNextPoint;

			// sprawdzamy, czy możemy poruszyć się w żądanym kierunku
			foreach (Vector2 vect in vectToNextPoint) if (vect == dir) return true;
		}

		return false; // jeśli nie mżemy poruszyć się w żądanym kierunku
	}

	// publiczna metoda do zmieniania wirusa w niebieskiego wirusa
	public void turnVirusBlue()
    {
		StartCoroutine(TurnVirusBlueAndBack());
    }

	IEnumerator TurnVirusBlueAndBack()
    {
		isVirusBlue = true; // wirus nie jest animowany gdy staje się niebieski
		sr.sprite = blueVirus; // zmiana grafiki wirusa na niebieską
		yield return new WaitForSeconds(6.0f); // czekamy 6 sek i przywracamy pierwotny stan wirusa
		isVirusBlue = false; // wirus może już być animwany
    }
}
