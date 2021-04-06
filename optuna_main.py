import numpy as np
import optuna

def objective(trial):
    x = list(range(81))
    for i, _ in enumerate(x):
        x[i] = trial.suggest_int('x' + str(i), 0, 1)
    print(sum(x))
    return sum(x)

optuna.logging.disable_default_handler()
study = optuna.create_study(direction = 'maximize')
study.optimize(objective, n_trials = 6 * 50)
print(study.best_value)
