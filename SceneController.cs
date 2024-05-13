using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq; //Used for shuffling function

public class SceneController : MonoBehaviour
{
    public int gridRows = 2;
    public int gridCols = 4;
    public float offsetX = 2f;
    public float offsetY = 2.5f;

    private int score = 0;

    private Vector3 startPos;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;

    //Create fields in the inspector
    [SerializeField] MemoryCard originalCard;
    [SerializeField] Sprite[] images;
    [SerializeField] TMP_Text scoreLabel;
    //Initialize deck of cards
    [SerializeField] int[] cardIdsForDeck = { 0, 1, 2, 3 };

    //Variables to shuffle cards midgame
    public float shuffleInterval = 5f;
    private float timer = 0f;
    private TMP_Text timerText;

    //Audio variables
    private AudioController audioController;
    public AudioClip MatchAudio;
    public AudioClip ShuffleAudio;
    public AudioClip WrongMatchAudio;

    public bool canReveal
    {
        get { return secondRevealed == null; }
    }

    public void CardRevealed(MemoryCard card)
    {
        if (firstRevealed == null)
        {
            firstRevealed = card;
        }
        else
        {
            secondRevealed = card;
            StartCoroutine(CheckMatch());
            //Debug.Log("Match? " + (firstRevealed.Id == secondRevealed.Id));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Audio controller initilization
        audioController = GameObject.FindObjectOfType<AudioController>();
        //Restart audio if it is not playing
        audioController.RestartGameMusic();

        //Start timer text
        timerText = GetComponentInChildren<TMP_Text>();
        UpdateTimerDisplay();

        //Set up the cards on the table
        Vector3 startPos = originalCard.transform.position;

        //int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        int[] numbers = ProduceDeck(cardIdsForDeck);
        numbers = ShuffleArray(numbers);
        //Counter for cards placed
        int cardsAdded = 0;

        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                //Skip for loop if it exceeds number of cards
                if (cardsAdded >= numbers.Length)
                {
                    continue;
                }

                MemoryCard card;

                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                //int index = j * gridCols + i;
                int index = cardsAdded;
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);

                //Debug line
                Debug.Log($"Placing card {cardsAdded} with ID {id} at position {i}, -{j} from top-left card.");
                cardsAdded++;
            }
        }
        //Shuffle cards with second shuffling algorithm to use that positiong system
        ShuffleCards();

        //Start timer for shuffling cards midgame
        StartCoroutine(ShuffleTimer());
    }

    //Algorithm to shuffle the array
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int temp = newArray[i];
            int rand = Random.Range(i, newArray.Length);
            newArray[i] = newArray[rand];
            newArray[rand] = temp;
        }
        return newArray;
    }

    //Method to produce deck of cards
    private int[] ProduceDeck(int[] numbers)
    {
        int[] newArray = new int[numbers.Length * 2];
        for (int i = 0; i < newArray.Length; i += 2)
        {
            newArray[i] = numbers[i / 2];
            newArray[i + 1] = numbers[i / 2];
        }
        return newArray;
    }

    //Check if the cards clicked match
    private IEnumerator CheckMatch()
    {
        if (firstRevealed.Id == secondRevealed.Id)
        {
            score++;
            audioController.PlayMatchMadeSE(MatchAudio);
            //Debug.Log($"Score: {score}");
            scoreLabel.text = $"Score: {score}";
        }
        else
        {
            audioController.PlayWrongMatchSE(WrongMatchAudio);
            yield return new WaitForSeconds(0.5f);
            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }

        firstRevealed = null;
        secondRevealed = null;
    }

    //Coroutine to call mid-game shuffle function every set of seconds
    private IEnumerator ShuffleTimer()
    {
        while (true)
        {
            timer = shuffleInterval;

            while (timer > 0f)
            {
                yield return new WaitForSeconds(1f);
                timer -= 1f;
                UpdateTimerDisplay();
            }

            ShuffleCards();
            timer = shuffleInterval;
        }
    }

    //Function to update time on timer
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        timerText.text = string.Format("Shuffle Countdown: {0:00}:{1:00}", minutes, seconds);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");
    }

    //Function to shuffle cards during the game
    public void ShuffleCards()
    {
        audioController.PlayShuffleSE(ShuffleAudio);
        //Get instances of cards in the scene
        MemoryCard[] cards = FindObjectsOfType<MemoryCard>();

        //Shuffle the array of cards
        int[] shuffledIndexes = ShuffleArray(Enumerable.Range(0, cards.Length).ToArray());

        //Reposition the freshly shuffled cards
        for (int i = 0; i < cards.Length; i++)
        {
            //Position shuffled cards
            int index = shuffledIndexes[i];
            float posX = ((offsetX * (index % gridCols)) + startPos.x) - 3;
            float posY = -((offsetY * (index / gridCols)) + startPos.y) + 1;

            cards[i].transform.position = new Vector3(posX, posY, startPos.z);
        }
    }
}
