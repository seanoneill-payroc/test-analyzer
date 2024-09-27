<script>
    import {onMount} from "svelte"
    let data = null;
    let loading = true;
    let error = null;

    // Fetch data when the component is created
    onMount(async () => {
      try {
        const response = await fetch('/api/testcasereport'); // Replace with your API endpoint
        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
        data = await response.json();
      } catch (err) {
        error = err.message;
      } finally {
        loading = false;
      }
    });
  </script>
  
  {#if loading}
    <p>Loading...</p>
  {:else if error}
    <p>Error loading data: {error}</p>
  {:else if data}
    <table>
      <thead>
        <tr>
          <!-- Table headers matching the keys in the payload -->
          <th>Test Case ID</th>
          <th>Project</th>
          <th>Title</th>
          <th>Runs</th>
          <th>Skipped</th>
          <th>Not Run</th>
          <th>Passed</th>
          <th>Failed</th>
          <th>Blocked</th>
          <th>Success Rate (%)</th>
        </tr>
      </thead>
      <tbody>
        {#each data as item}
        <tr>
          <!-- Display data values -->
          <td><a href="https://app.testiny.io/{item.project}/testcases/tc/{item.testCaseId.substring(3)}/edit" target="testiny">{item.testCaseId}</a></td>
          <td>{item.project}</td>
          <td>{item.title}</td>
          <td>{item.runs}</td>
          <td>{item.skipped}</td>
          <td>{item.notRun}</td>
          <td>{item.passed}</td>
          <td>{item.failed}</td>
          <td>{item.blocked}</td>
          <td 
            class:warning={item.successRate >= 70 && item.successRate < 90}
            class:error={item.successRate < 70}
            >{item.successRate}</td>
        </tr>
        {/each}
      </tbody>
    </table>
  {/if}
  
  <style>
        body { font-size: 10pt; }
        table {
            width: 100%;
            border-collapse: collapse;
            margin: 25px 0;
            font-size: .8em;
            text-align: left;
            box-shadow: 0 2px 15px rgba(64, 64, 64, 0.1);
            background-color: white;
        }
        thead {
            box-shadow: 0 2px 15px rgba(64, 64, 64, 0.1);
            background-color: white;
            position: sticky;
            top:0px;
            border-bottom: #009879;
        }
        th, td {
            padding: 12px 15px;
        }

        th {
            text-transform: uppercase;
            letter-spacing: 0.03em;
        }

        tr {
            border-bottom: 1px solid #dddddd;
        }

        tr:nth-of-type(even) {
            background-color: #f3f3f3;
        }

        tr:last-of-type {
            border-bottom: 2px solid #009879;
        }

        tr:hover {
            background-color: #f1f1f1;
        }

        /* Responsive Design */
        @media (max-width: 600px) {
            table, thead, tbody, th, td, tr {
                display: block;
            }

            th {
                position: absolute;
                top: -9999px;
                left: -9999px;
            }

            tr {
                margin-bottom: 10px;
                border: 1px solid #ddd;
            }

            td {
                border: none;
                position: relative;
                padding-left: 50%;
                text-align: right;
            }

            td:before {
                content: attr(data-label);
                position: absolute;
                left: 0;
                width: 50%;
                padding-left: 15px;
                font-weight: bold;
                text-align: left;
            }
        }


    .error {
        color:red;
    }
    .warning {
        color:pink;
    }
    table {
      width: 100%;
      border-collapse: collapse;
    }
    th, td {
      border: 1px solid #ccc;
      padding: 8px;
      text-align: left;
    }

    th:nth-child(5),td:nth-child(5) {
        background-color: lightgray;
    } /*skipped*/
    th:nth-child(6),td:nth-child(6) {
        background-color: lightgray;
    } /*NotRun*/
    th:nth-child(7),td:nth-child(7) {
        background-color: lightgreen;
    } /*passed*/
    th:nth-child(8),td:nth-child(8){ 
        background-color: pink;
        color:white;
    }/*failed*/
    th:nth-child(9),td:nth-child(9) {
        background-color: pink;
        color:white;
    } /*blocked*/
  </style>
  