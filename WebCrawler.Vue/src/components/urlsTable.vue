<template>
  <div>
    <h3 class="elementInTheCenter">{{page.title}}</h3>
    <table class="table table-striped table-bordered table-active">

        <thead class="thead-dark">
            <tr>
                <th>Url</th>
            </tr>
        </thead>

        <tr v-for="item in page.urlList" :key="item.id">
            <td>{{item.url}}</td>
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