const initState = () => ({
  tricks: []
});

export const state = initState;

export const getters = {
  trickItems: state => state.tricks.map(x => ({
    text: x.trickName,
    value: x.id
  }))
}

export const mutations = {
  setTricks(state, tricks){
    state.tricks =  tricks
  },
  reset(state){
    Object.assign(state, initState())
  }
}

export const actions = {
  async fetchTricks({commit}){
    const tricks =  await this.$axios.$get("/api/tricks")
    console.log("Tricks: ", tricks)
    commit("setTricks", tricks)
  },
   createTrick({commit, dispatch, state}, {form}) {
      return this.$axios.$post("/api/tricks", form);

  }

}
