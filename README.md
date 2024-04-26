NormalizedText Class
Represents text data normalized into proportion vectors for language analysis.
Provides methods to set up proportions based on text content or file input.

Perceptron Class
Represents a simple perceptron model for language classification.
Includes methods for calculation, learning, and decision-making based on input vectors.

Program Class
Implements the main logic for training and testing language classification using perceptrons.
Handles data transformation, perceptron creation, training, testing, and result evaluation.
Features

Data Processing: Converts raw text data into normalized proportion vectors for training and testing.
Perceptron Creation: Dynamically creates perceptrons based on available languages for training.
Training and Testing: Trains perceptrons using training data and tests their accuracy on test data.
Decision Making: Makes language predictions based on input text using trained perceptrons.

Main Method
Data Preparation: Normalizes text data, creates perceptrons, and initializes training and test datasets.
Training: Iteratively trains perceptrons until convergence or a set number of epochs.
Testing: Evaluates perceptron accuracy on test data and prints results.
User Interaction: Allows users to input text for language prediction using the trained perceptrons.
