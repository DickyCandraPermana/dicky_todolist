<script setup lang="ts">
import { RouterLink } from "vue-router";
</script>

<template>
  <header
    class="flex flex-row items-center justify-between bg-blue-400 text-white py-2 px-5"
  >
    <router-link to="/"><img src="" alt="some img" class="h-8" /></router-link>
    <nav class="flex flex-row gap-4">
      <router-link to="/register">Register</router-link>
      <router-link to="/login">Login</router-link>
    </nav>
  </header>
</template>
