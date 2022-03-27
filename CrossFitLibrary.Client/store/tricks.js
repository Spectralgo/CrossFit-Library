const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: [],
});

export const state = initState;

// Allow us to build new data from state values
export const getters = {
  trickById: state => id => state.tricks.find(trick => trick.slug === id),
  categoryById: state => id => state.categories.find(category => category.slug === id),
  difficultyById: state => id => state.difficulties.find(difficulty => difficulty.slug === id),
  trickItems: state => state.tricks.map(x => ({
    text: x.name,
    value: x.slug
  })),
  categoryItems: state => state.categories.map(x => ({
    text: x.name,
    value: x.slug
  })),
  difficultyItems: state => state.difficulties.map(x => ({
    text: x.name,
    value: x.slug
  })),
}

export const mutations = {
  setTricks(state, {tricks, categories, difficulties}) {
    state.tricks =  tricks
    state.categories =  categories
    state.difficulties =  difficulties
  },
  reset(state){
    Object.assign(state, initState())
  }
}

export const actions = {

  // Gets called by nuxt at first load in the index store before rendering
  // Calls the Dotnet Api to get list of all tricks, categories and difficulties and store them locally in this store
  async fetchTricks({commit}){
    try{
      const tricks =  await this.$axios.$get("/api/tricks")
      const categories =  await this.$axios.$get("/api/categories")
      const difficulties =  await this.$axios.$get("/api/difficulties")
      console.log("Tricks:", tricks,"\nCategories: ", categories,"\nDifficulties: ", difficulties)
      commit("setTricks",{tricks, categories, difficulties})
    }catch (err){
      console.log('error from fetch:',err)
    }
  },
   createTrick({commit, dispatch, state}, {form}) {
    console.log("Create Trick: ", form)
      return this.$axios.$post("/api/tricks", form);

  },
  updateTrick({commit, dispatch, state}, {form}) {
    console.log("Create Trick: ", form)
    return this.$axios.$put("/api/tricks", form);

  },
  createCategory({commit, dispatch, state}, {form}) {
    return this.$axios.$post("/api/categories", form);

  },
  createDifficulty({commit, dispatch, state}, {form}) {
    return this.$axios.$post("/api/difficulties", form);

  },

}
