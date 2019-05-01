import random
import copy
import os

def chunks(l, n):
    """Yield successive n-sized chunks from l."""
    for i in range(0, len(l), n):
        yield l[i:i + n]

def generate_goal_config(n=4):
    values = list(map(lambda x: str(x),range(1,(n**2)-1))) + 2 * ['-']
    random.shuffle(values)
    return [chunk for chunk in chunks(values,n)]

def make_random_move(j, i, config):
    config = copy.deepcopy(config)
    m = len(config) - 1
    moves = []
    if i != 0:
        moves.append('left')
    if i != m:
        moves.append('right')
    if j != 0:
        moves.append('up')
    if j != m:
        moves.append('down')
    if len(moves):
        move = random.choice(moves)
        if move == 'left':
            config[j][i-1], config[j][i] = config[j][i], config[j][i-1]
        if move == 'right':
            config[j][i+1], config[j][i] = config[j][i], config[j][i+1]
        if move == 'up':
            config[j-1][i], config[j][i] = config[j][i], config[j-1][i]
        if move == 'down':
            config[j+1][i], config[j][i] = config[j][i], config[j+1][i]
    return config

def generate_input_config(configuaration):
    number_of_moves = random.randint(1,30)
    m = len(configuaration)
    n = 0
    while n < number_of_moves:
        for j in range(m):
            for i in range(m):
                if configuaration[j][i] == '-':
                    configuaration = make_random_move(j, i, configuaration)
                    n += 1
    return configuaration

def write_configuartion(file_name, configuaration):
    with open(file_name,'w') as file:
        lines = ['\t'.join(line) for line in configuaration]
        data = '\n'.join(lines)
        file.write(data)

def write_files(base_name, goal_configuaration, input_configuaration):
    write_configuartion(f'{base_name}_goal.txt', goal_configuaration)
    write_configuartion(f'{base_name}_input.txt', input_configuaration)

def run_test(test_name, goal_configuaration, input_configuaration):
    write_files(test_name, goal_configuaration, input_configuaration)
    os.system(f'N-Puzzle.exe {test_name}_input.txt {test_name}_goal.txt {test_name}_output.txt')

if __name__ == "__main__":
    goal_configuaration = generate_goal_config(4)
    input_configuration = generate_input_config(goal_configuaration)
    run_test('test', goal_configuaration, input_configuration)
    