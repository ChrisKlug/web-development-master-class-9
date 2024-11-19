<script lang="ts">
	import Cart from '$lib/components/Cart.svelte';
	import NavBar from '$lib/components/NavBar.svelte';
	import Profile from '$lib/components/Profile.svelte';
	import { Toaster } from '$lib/components/ui/sonner';
	import '../app.css';
	import type { LayoutData } from './$types';

	export let data: LayoutData;
</script>

<main>
	<Toaster />
	<div class="flex h-16 items-center px-4">
		<NavBar />
		<div class="ml-auto flex items-center space-x-4">
			<Cart cart={data.cart} />
			<Profile user={data.user} />
		</div>
	</div>
	<slot />
</main>
