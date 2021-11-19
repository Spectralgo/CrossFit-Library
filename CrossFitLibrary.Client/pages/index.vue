<template>
  <div>
    <v-card class="pa-8 ">
      <div v-if="tricks">
        <div v-for="t in tricks">
          <v-btn :to="`/tricks/${t.id}`">
            {{ t.trickName }}
          </v-btn>
        </div>
      </div>
    </v-card>
  </div>
</template>

<script>
import {mapState} from 'vuex';

export default {
  data: () => ({}),

  computed: {
    ...mapState('tricks', ['tricks']),
    ...mapState('submissions', ['submissions']),
  },
}

</script>
