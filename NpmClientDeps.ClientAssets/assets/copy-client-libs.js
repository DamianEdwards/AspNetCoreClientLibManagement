import { argv } from 'node:process';
import fs from 'node:fs/promises';

// set output dir
const wwwroot = './../wwwroot/';
const outputDir = (argv.length >= 3 ? argv[2] : wwwroot) + 'lib';
console.info(`Output dir: ${outputDir}`);

const libs = {
    bootstrap: {
        src: './node_modules/$name/dist',
        dest: `${outputDir}/$name/dist`
    },
    jquery: {
        src: './node_modules/$name/dist',
        dest: `${outputDir}/$name/dist`
    },
    ['jquery-validation']: {
        src: './node_modules/$name/dist',
        dest: `${outputDir}/$name/dist`
    },
    ['jquery-validation-unobtrusive']: {
        src: './node_modules/$name/dist',
        dest: `${outputDir}/$name/dist`
    }
};

for (const key in libs) {
    try {
        const lib = libs[key],
            src = lib.src.replace('$name', key),
            dest = lib.dest.replace('$name', key);

        // delete dest if it already exists
        try {
            await fs.rm(dest, { force: true, recursive: true });
        } catch { }

        // copy src to dest
        await fs.cp(src, dest, { recursive: true });

        console.log(`Library ${key} copied from ${src} to ${dest}`);
    } catch (error) {
        console.error(`Library ${key} could not be copied: ${error}`);
    }
}
