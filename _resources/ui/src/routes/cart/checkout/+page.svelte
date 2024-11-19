<script lang="ts">
	import type { PageData } from './$types';
	import CheckoutForm from '$lib/components/CheckoutForm.svelte';

	export let data: PageData;
</script>

<svelte:head>
    <title>Goods and Greens | Checkout</title> 
</svelte:head>

<div class="p-4">
	<CheckoutForm data={data.form} cart={data.cart} />
</div>
