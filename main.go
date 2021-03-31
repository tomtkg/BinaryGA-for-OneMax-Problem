package main

import (
	"fmt"
	"math"
	"math/rand"
	"sort"
	"time"
)

type Individual struct {
	fitness int
	gene    []bool
}

func makePopulation(genes [][]bool) []Individual {
	pop := make([]Individual, len(genes))
	for i, v := range genes {
		pop[i] = Individual{fitness: oneMax(v), gene: v}
	}
	return pop
}
func onemax(gene []bool) int {
	count := 0
	for _, v := range gene {
		if v {
			count++
		}
	}
	return count
}
func main() {
	rand.Seed(time.Now().UnixNano())
	param := map[string]int{
		"dim":          81,
		"eliteSize":    5,
		"childSize":    6,
		"popSize":      11,
		"crossover":    50,
		"mutation":     4,
		"maxIteration": 50,
	}
	onemaxGA(param)
}
func onemaxGA(param map[string]int) {
	firstGenes := make([][]bool, param["eliteSize"])
	for i := range firstGenes {
		firstGenes[i] = make([]bool, param["dim"])
		for j := range firstGenes[i] {
			firstGenes[i][j] = rand.Float64() < 0.1
		}
	}
	pop := makePopulation(firstGenes)
	sort.Slice(pop, func(i, j int) bool { return pop[i].fitness > pop[j].fitness })
	fmt.Println(pop[0].fitness)
	for iteration := 2; iteration < param["maxIteration"]; iteration++ {
		genes := make([][]bool, param["childSize"])
		for i := 0; i < param["childSize"]; i += 2 {
			genes[i] = append([]bool{}, pop[rand.Intn(param["eliteSize"])].gene...)
			genes[i+1] = append([]bool{}, pop[rand.Intn(param["eliteSize"])].gene...)
			crossOverPoints := rand.Perm(param["dim"])[:param["dim"]*param["crossover"]/100]
			for _, v := range crossOverPoints {
				genes[i][v], genes[i+1][v] = genes[i+1][v], genes[i][v]
			}
		}
		for i := range genes {
			genes[i] = mutation(genes[i], float64(param["mutation"]))
		}
		pop = append(pop, makePopulation(genes)...)
		sort.Slice(pop, func(i, j int) bool { return pop[i].fitness > pop[j].fitness })
		pop = pop[:param["eliteSize"]]
		fmt.Println(pop[0].fitness)
	}
}
func mutation(gene []bool, mu float64) []bool {
	trues := []int{}
	falses := []int{}
	for i, v := range gene {
		if v {
			trues = append(trues, i)
		} else {
			falses = append(falses, i)
		}
	}
	gene = change(gene, mu, trues)
	gene = change(gene, mu, falses)
	return gene
}
func change(gene []bool, mu float64, ids []int) []bool {
	rand.Shuffle(len(ids), func(i, j int) { ids[i], ids[j] = ids[j], ids[i] })
	for _, v := range ids[:min(poissonRand(mu), len(ids))] {
		gene[v] = !gene[v]
	}
	return gene
}
func poissonRand(lambda float64) int {
	L := math.Pow(math.E, -lambda)
	k := 0
	p := 1.0
	for p > L {
		k++
		p *= rand.Float64()
	}
	return (k - 1)
}
func min(a, b int) int {
	if a < b {
		return a
	}
	return b
}
