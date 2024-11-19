<script lang="ts">
	import { page } from '$app/stores';
</script>

<svelte:head>
	<title>Whoopsi Daisy!</title>
</svelte:head>

<div class="p-4">
	<div role="alert">
		<div class="rounded-t bg-red-500 px-4 py-2 font-bold text-white">Whoopsi Daisy!</div>
		<div class="rounded-b border border-t-0 border-red-400 bg-red-100 px-4 py-3 text-red-700">
			<p>
				Something not ideal might be happening: <span class="italic">{$page.error?.message}</span>
			</p>
		</div>
	</div>
</div>
