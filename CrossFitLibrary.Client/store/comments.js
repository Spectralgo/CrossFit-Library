const initState = () => ({
  comments: []
});

export const state = initState;

export const mutations = {
  setComments(state, comments){
    state.comments =  comments
  },
  reset(state){
    Object.assign(state, initState())
  }
}

export const actions = {
  async fetchCommentsForTrick({commit}, {modId}){
    await this.$axios.$get(`api/moderation-items/${modId}/comments`)
  },
  createComment({commit, state}, {trickId, comment}) {
  }

}
