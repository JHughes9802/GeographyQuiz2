using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeographyQuiz2
{
    public partial class Form1 : Form
    {
        // Instantiation of the Questions and Answers lists
        private readonly List<string> Questions = new List<string> {"What is the state capital of California?",
            "What is the tallest mountain on land in the world?",
            "Which country has the furthest south extent?"};

        private readonly List<string> Answers = new List<string> {"Sacramento",
            "Everest",
            "Chile"};

        private int QuestionNumber = -1; // Counter to keep track of the current question/answer
        private int Score = 0;
        private StringBuilder IncorrectAnswersBuilder = new StringBuilder();
        DateTime startTime = DateTime.Now; // This gets the start time right when the form is opened

        public Form1()
        {
            InitializeComponent();
            ShowNextQuestion();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblScore.Text = $"Score: {Score}"; // This porperly sets lblScore on form load
        }

        private void ShowNextQuestion()
        {
            QuestionNumber++;

            // I considered using a foreach loop, but couldn't manage to get it to work how I wanted
            if (QuestionNumber < Questions.Count)
            {
                lblQuestion.Text = Questions[QuestionNumber];
                // QuestionNumber++ can fit here if it starts at 0 and the below statement has + 1 removed, but it looks strange
                // I added this so the user can know how far they are in the quiz - it's nice for long quizzes
                lblQuestionNumber.Text = $"Question: {QuestionNumber + 1}/{Questions.Count}";

                // This is basic QoL (Quality of Life) that I like adding when possible
                txtAnswer.Clear();
                txtAnswer.Focus();
            }

            else
            {
                QuizOver();

                Close();
            }
        }

        // I created the QuizOver method because there was too much unrelated code in ShowNextQuestion
        private void QuizOver()
        {
            btnSubmitAnswer.Enabled = false;

            // Given how short the quiz is, I felt it proper to include seconds
            TimeSpan timeTaken = DateTime.Now - startTime;
            int minutes = timeTaken.Minutes;
            int seconds = timeTaken.Seconds;

            /* I use an if/else because it's improper to have the line 
             * "Incorrect Answers" display if they got them all correct */
            if (Score == Questions.Count)
            {
                MessageBox.Show($"Your score is {Score}" +
                    $"\nIt took {minutes} minutes and {seconds} seconds to finish",
                    "Quiz Over!");
            }

            /* This formatting works fine since it's a short quiz, but it'd need to be changed when
             * there's more than 5 questions because the message box could get cut off */
            else
            {
                string incorrectAnswers = IncorrectAnswersBuilder.ToString();
                MessageBox.Show($"Your score is {Score}" +
                    $"\nIt took {minutes} minutes and {seconds} seconds to finish" +
                    $"\n\nIncorrect Answers {incorrectAnswers}",
                    "Quiz Over!");
            }
        }

        private void CheckAnswer()
        {
            string answer = txtAnswer.Text.Trim(); // Included the trim function to increase accuracy

            /* I know I could do this with a sorted list, but I wanted to see if I could make things
             * work instead with two separate lists. I'd still prefer a sorted list mainly because
             * I can easily see if question and answer are at the same index if there's many questions */
            // I do Length + 5 to attempt to in case the user adds " city" or "Mt. "
            // I'm curious what you'd do/suggest to check for correct answers
            if (answer.Contains(Answers[QuestionNumber]) &&
                answer.Length <= Answers[QuestionNumber].Length + 5)
            {
                MessageBox.Show("Correct!", "Result");
                Score++;
                // I added this so the user can know their total score during the quiz
                lblScore.Text = $"Score: {Score}";
            }

            else
            {
                MessageBox.Show("Incorrect", "Result");

                // I decided to use a MessageBuilder for this quiz app, I like how the output came out
                IncorrectAnswersBuilder.Append("\n\nQuestion: ");
                IncorrectAnswersBuilder.Append(Questions[QuestionNumber]);
                IncorrectAnswersBuilder.Append("\nYour answer: ");
                IncorrectAnswersBuilder.Append(answer);
                IncorrectAnswersBuilder.Append("\nCorrect answer: ");
                IncorrectAnswersBuilder.Append(Answers[QuestionNumber]);
            }
        }

        private void btnSubmitAnswer_Click(object sender, EventArgs e)
        {
            /* This is to ensure the user entered an answer before checking if their answer
             * is correct because it'd be improper to tell them they got it wrong if they
             * accidentally pressed the enter key before entering anything */
            if (String.IsNullOrWhiteSpace(txtAnswer.Text))
            {
                MessageBox.Show("Please enter an answer", "Error");
                txtAnswer.Focus();
            }

            else
            {
                CheckAnswer();
                ShowNextQuestion();
            }
        }
    }
}
