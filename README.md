Ratatat
=======

This is an expirement that I wrote when I discovered the world of evolutionary algorithms. These are algorithms that
solve problems by emulating some aspects of real evolutionary biology. The problem I used to test my understanding
of a Genetic Algorithm was a simple problem of finding a path to some cheese from a starting point.

This demonstration allows you create a maze, 
place a cheese somewhere in the maze and watch as your rats learn over the generations of evolution.

How it works
---------
Each rat has a genome. This genome is made up of many chromosomes that are in turn made up of individual genes. 
The encoding for a gene is a single bit and a chromosome consists of two genes (or 2 bits). Therefore each chromosome
has 4 possible encodings, 11,01,10,00. Each one of these encodings maps to a decision made by the rat to go up, down
left or right. Since a genome is made up of many chromosomes, a genome can be seen as a sequence of decisions on
which direction to move. 

Now consider a large population of rats with randomly generated genomes, a maze in the world and some cheese hidden in this
maze. We test each rat in this maze, using its genome. Every rat is assigned a fitness score based on how well it 
did. This can be a simple calculation of how close it made it to the cheese or more complicated. In my case,
the rats fitness is a function of how close it made it to the cheese (the closer the higher the fitness), 
with penalties for how many times it ran into a wall, and how many times it stepped on a tile that it had already 
traversed.

Now that each rat in the population has had a trial in the maze we will call this a generation. The next 
generation/population of rats will be chosen from this current population. Rats will be chosen randomly
in such a way that their chance of being selected for the next generation is proportional to their fitness score;
rats that did good will be more likely to be selected then rats that did poorly. This type of selection
is called roulette wheel selection. 

Now lets say I pick exactly two rats from the population using roullete wheel selection. These rats will mate with
each other 70% of time. This chance is called the cross over rate. What does mating mean in this context? It means their
genomes (which is just a string of bits) will get sliced in two pieces from a random pivot and each rat will swap
one of their pieces. This will produce two new rats, which we will call the off spring of the two original rats 
selected. After cross over, comes mutation this is a very small chance (ussually around 1%) that 1 or more bits are 
flipped in the process of cross over.
