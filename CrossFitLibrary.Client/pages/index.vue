<template>
  <div>
    <v-card class="pa-8 ">
      <div v-if="tricks">
        <div v-for="t in tricks">
          {{ t.name }}
          <div>
            <video :src="`http://localhost:5000/api/video/${t.video}`" controls width="400"></video>
          </div>
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
    ...mapState('tricks', ['tricks'],
    ),
  }
}

</script>
