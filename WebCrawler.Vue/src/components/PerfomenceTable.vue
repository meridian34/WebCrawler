<template>
  <div>
    <h3 class="elementInTheCenter">PERFOMANCE</h3>
    <table class="table table-striped table-bordered table-active">
      <thead class="thead-dark">
        <tr>
          <th>Url</th>
          <th>Elapsed Milliseconds</th>
        </tr>
      </thead>
       <tr v-for="item in page.links" :key="item.id">
          <td>{{item.url}}</td>
          <td>{{item.elapsedMilliseconds}}</td>
       </tr>
    </table>
  </div>
</template>

<script>
export default {
    props:['page'],
    
}
</script>

<style>

</style>