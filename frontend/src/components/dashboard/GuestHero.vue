<template>
  <section class="flex min-h-[60vh] flex-col items-center justify-center gap-2">
    <h1 class="text-6xl font-black text-slate-900 tracking-tight">Todo App</h1>
    <p class="text-xl text-slate-600">Align your productivity, one task at a time.</p>
    <div class="mt-8 flex gap-4">
      <router-link
        to="/login"
        class="rounded-full bg-slate-900 px-8 py-3 font-semibold text-white transition hover:bg-slate-800 shadow-md"
      >Get Started</router-link>
      <router-link
        to="/register"
        class="rounded-full border border-slate-200 px-8 py-3 font-semibold text-slate-600 transition hover:bg-slate-50"
      >Create Account</router-link>
    </div>
  </section>
</template>
