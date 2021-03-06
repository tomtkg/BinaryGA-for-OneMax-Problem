function Main()
    Param.Dim = 81;
    Param.EliteSize = 5;
    Param.ChildSize = 6;
    Param.PopSize = Param.EliteSize + Param.ChildSize;
    Param.Crossover = 0.5;
    Param.Mutation = 4;
    Param.MaxIteration = 50;
    OneMax = @(x) sum(x, 2);
    OneMaxGA(Param,OneMax);
end
function OneMaxGA(Pram,OneMax)
    Gene = rand(Pram.PopSize, Pram.Dim) < 0.5;
    Fitness = OneMax(Gene);
    Pop = table(Fitness, Gene);
    Pop = sortrows(Pop, 'Fitness');
    disp(Pop.Fitness(end));
    for Iteration = 2 : Pram.MaxIteration
        Parents = randi([Pram.ChildSize+1 Pram.PopSize], Pram.ChildSize, 1);
        Pop.Gene(1 : Pram.ChildSize, :) = Pop.Gene(Parents, :);
        for i = 1 : 2 : Pram.ChildSize
            Flag = rand(Pram.Dim,1) < Pram.Crossover;
            Pop.Gene([i i+1], Flag) = Pop.Gene([i+1 i], Flag);
        end
        for i = 1 : Pram.ChildSize
            Flag = Pop.Gene(i, :);
            Pop.Gene(i, Flag) = Mutation(Pop.Gene(i, Flag), Pram.Mutation);
            Pop.Gene(i,~Flag) = Mutation(Pop.Gene(i,~Flag), Pram.Mutation);
        end
        Pop.Fitness(1 : Pram.ChildSize) = OneMax(Pop.Gene(1 : Pram.ChildSize, :));
        Pop = sortrows(Pop, 'Fitness');
        disp(Pop.Fitness(end));
    end
end
function Array = Mutation(Array, Lambda)
    N = size(Array,2);
    Ids = randperm(N);
    count = min([poissrnd(Lambda), N]);
    Array(Ids(1:count)) = ~Array(Ids(1:count));
end
