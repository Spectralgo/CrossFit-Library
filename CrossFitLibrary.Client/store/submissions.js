import {UPLOAD_TYPE} from "@/data/enum";

const initState = () => ({
  submissions: []
});

export const state = initState;

export const mutations = {
  setSubmissions(state, submissions){
    state.submissions =  submissions
  },
  reset(state){
    Object.assign(state, initState())
  }
}

export const actions = {
  async fetchSubmissionsForTrick({commit}, {trickId}){
    console.log("trickid: ", trickId)
    const submissions =  await this.$axios.$get(`/api/tricks/${trickId}/submissions`)
    commit("setSubmissions", submissions)
    console.log("Submissions: ", submissions)
  },
  createSubmission({commit, dispatch, state}, {form}) {
    return this.$axios.$post("/api/submissions", form );
  }

}
