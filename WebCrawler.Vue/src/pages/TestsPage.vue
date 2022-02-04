<template>
  <div>
    <!-- input component -->
    <crawling-component></crawling-component>
    
    <!-- loading animation -->
    <loading-component ></loading-component>

    <!-- table block -->
    <tests-table-component></tests-table-component>
  </div>
</template>

<script>

import loadingComponent from '../components/Loading.vue'
import crawlingComponent from '../components/CrawlingForm.vue'
import testsTableComponent from '../components/TestsTable.vue'

export default {

  components:{
    'loadingComponent' : loadingComponent,
    'crawlingComponent' : crawlingComponent,
    'testsTableComponent' : testsTableComponent
  },
};
</script>

<style>

</style>