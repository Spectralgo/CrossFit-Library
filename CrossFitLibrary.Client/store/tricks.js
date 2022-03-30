const initState = () => ({
  dictionary: {
    tricks: null,
    categories: null,
    difficulties: null,
  },
  lists: {
    tricks: [],
    categories: [],
    difficulties: [],
  },
});

export const state = initState;

const setEntities = (state ,type, entities) => {
  state.dictionary[type] = {}
  entities.forEach( entity => {
    state.lists[type].push(entity)
    state.dictionary[type][entity.id] = entity

    if (entity.slug){//only tricks have slugs
      state.dictionary[type][entity.slug] = entity
    }
    console.log(entity)
  })
}


export const mutations = {
  setTricks(state, {tricks}) {
    setEntities(state, 'tricks', tricks)
  },
  setDifficulties(state, {difficulties}) {
    setEntities(state, 'difficulties', difficulties)
  },
  setCategories(state, {categories}) {
    setEntities(state, 'categories', categories)
  },
  reset(state){
    Object.assign(state, initState())
  }
}

export const actions = {
  // Gets called by nuxt at first load in the index store before rendering
  // Calls the Dotnet Api to get list of all tricks, categories and difficulties and store them locally in this store
  fetchTricks({commit}){
    return Promise.all([
      this.$axios.$get("/api/tricks").then(tricks => commit('setTricks',{tricks})),
      this.$axios.$get("/api/difficulties").then(difficulties => commit('setDifficulties',{difficulties})),
      this.$axios.$get("/api/categories").then(categories => commit('setCategories',{categories})),
    ])
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
